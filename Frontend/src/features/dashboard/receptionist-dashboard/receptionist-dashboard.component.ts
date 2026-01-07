import { CommonModule, DatePipe } from '@angular/common';
import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';
import { ReceptionistService } from '../../../core/services/receptionist-service';
import { Patient } from '../../../types/patient-types';
import { AppointmentDto } from '../../../types/appointment-types';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Doctor } from '../../../types/user-types';
import { AppointmentService } from '../../../core/services/appointment-service';
import { Subject, switchMap } from 'rxjs';

@Component({
  selector: 'app-receptionist-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe],
  templateUrl: './receptionist-dashboard.component.html'
})
export class ReceptionistDashboardComponent implements OnInit {
  public auth = inject(AuthService);
  private receptionistService = inject(ReceptionistService);
  private appointmentService = inject(AppointmentService);
  private fb = inject(FormBuilder)
  protected today = new Date();

  activeSection = signal<string>('queue');
  patients = signal<Patient[]>([]);
  doctors = signal<Doctor[]>([]);
  selectedDate = signal<Date>(new Date());
  selectedDoctorId = signal<string | null>(null);
  todayAppointments = signal<AppointmentDto[]>([]);
  appointments = signal<AppointmentDto[]>([]);
  isLoading = signal<boolean>(false);
  timeSlots = Array.from({ length: 11 }, (_, i) => i + 8);
  isModalOpen = signal(false);
  searchQuery = signal('');
  patientForm = this.fb.nonNullable.group({
    patientCode: ['', [Validators.required]],
    name: ['', [Validators.required]],
    gender: ['', [Validators.required]],
    age: [0, [Validators.required, Validators.min(0)]],
    birthDate: ['', [Validators.required]],
    phone: [''],
    email: ['', [Validators.email]],
    address: [''],
    emergencyContact: ['']
  });

  appointmentForm = this.fb.group({
    patientId: ['', [Validators.required]],
    doctorId: ['', [Validators.required]],
    startTime: ['', [Validators.required]],
    endTime: ['', [Validators.required]],
    status: ["Scheduled"],
    visitId: [null]
  });

  quickBookForm = this.fb.group({
    patientId: ['', Validators.required],
    patientName: ['', Validators.required],
    doctorId: ['', Validators.required],
    startTime: ['', Validators.required],
    duration: [30],
  });

  private refresh$ = new Subject<string>();
  constructor() {
    this.refresh$.pipe(
      switchMap(date => this.receptionistService.getAppointmentsByDate(date))
    ).subscribe(data => this.appointments.set(data));

    effect(() => {
      const dateStr = this.selectedDate().toISOString().split('T')[0];
      this.refresh$.next(dateStr);
    });
  }

  ngOnInit() {
    this.loadAllData();
  }

  openQuickBook(hour?: number) {
    this.quickBookForm.reset({
      doctorId: this.selectedDoctorId() || '',
      startTime: hour ? `2026-01-07T${hour.toString().padStart(2, '0')}:00` : '',
      duration: 30
    });
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
  }

  selectPatient(patient: any) {
    this.quickBookForm.patchValue({
      patientId: patient.id,
      patientName: patient.name
    });
    this.patients.set([]);
    this.searchQuery.set(''); 
  }

  changeDate(days: number) {
    const newDate = new Date(this.selectedDate());
    newDate.setDate(newDate.getDate() + days);
    this.selectedDate.set(newDate);
    this.receptionistService.getAppointments();
  }

  getCurrentTimeOffset(): number {
    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();

    // 如果当前时间不在诊所营业时间（8:00 - 18:00）内，可以隐藏红线或停留在顶部/底部
    if (hours < 8 || hours >= 18) {
      return -100; // 移出可视区域
    }

    // 计算逻辑：(当前小时 - 起始小时 8) * 每小时高度 80px + (分钟/60 * 80px)
    const offset = (hours - 8) * 80 + (minutes / 60) * 80;
    return offset;
  }

  getAppointmentStyle(apt: any) {
    const start = new Date(apt.startTime);
    const end = new Date(apt.endTime);

    if (isNaN(start.getTime()) || isNaN(end.getTime())) {
      console.error('Invalid date for appointment:', apt);
      return {};
    }
    const startHour = start.getHours() + (start.getMinutes() / 60);
    const endHour = end.getHours() + (end.getMinutes() / 60);

    const duration = endHour - startHour;
    const top = (startHour - 8) * 80;
    const height = duration * 80;

    return {
      'top': `${(startHour - 8) * 80}px`,
      'height': `${Math.max(duration * 80, 28)}px`,
      'position': 'absolute',
      'display': 'block',
      'z-index': '10'
    };
  }

  getStatusClass(status: string): string {
    if (!status) return 'bg-primary/10 border-primary';

    // 转换为小写进行比较，防止后端返回 "scheduled" 或 "Scheduled" 导致的匹配失败
    switch (status.toLowerCase()) {
      case 'scheduled':
        return 'bg-success/10 border-success text-success-content';
      case 'checkedin':
        return 'bg-info/10 border-info text-info-content';
      case 'completed':
        return 'bg-base-300 border-base-content/30 text-base-content/50';
      default:
        return 'bg-primary/10 border-primary text-primary-content';
    }
  }

  onSaveAppointment() {
    if (this.quickBookForm.invalid) return;

    const formValue = this.quickBookForm.value;

    // 1. 构造发送给后端的 Payload
    // 计算 EndTime (根据 StartTime + Duration)
    const startTime = new Date(formValue.startTime!);
    const endTime = new Date(startTime.getTime() + formValue.duration! * 60000);

    if (this.checkConflict(startTime, endTime, formValue.doctorId!)) {
      alert('Warning: This doctor already has an appointment at this time.');
      return;
    }

    const newAppointmentRequest = {
      patientId: formValue.patientId,
      doctorId: formValue.doctorId,
      startTime: startTime.toISOString(),
      endTime: endTime.toISOString(),
      status: 'Scheduled', // 初始状态为字符串
    };

    // 2. 调用后端 API
    this.receptionistService.createAppointment(newAppointmentRequest).subscribe({
      next: (savedApt: AppointmentDto) => {
        // 3. 乐观更新：将后端返回的带 ID 的 DTO 直接加入当前列表
        // 这样用户会立刻在日历上看到新生成的色块，无需刷新整个页面
        this.appointments.update(current => [...current, savedApt]);

        // 4. 重置与关闭
        this.isModalOpen.set(false);
        this.quickBookForm.reset();

        // 5. 成功提示 (使用简单的 Toast 或 Alert)
        // toast.success('Appointment booked for ' + savedApt.patientName);
      },
      error: (err) => {
        console.error('Save failed', err);
        alert('Failed to save appointment. Please check for scheduling conflicts.');
      }
    });
  }

  checkConflict(newStart: Date, newEnd: Date, doctorId: string): boolean {
    return this.appointments().some(apt => {
      if (apt.doctorId !== doctorId) return false;
      const s = new Date(apt.startTime);
      const e = new Date(apt.endTime);
      // 检查时间交叉：(StartA < EndB) && (EndA > StartB)
      return (newStart < e && newEnd > s);
    });
  }

  cancelAppointment(id: string) {
    if (confirm('Are you sure you want to cancel this appointment?')) {
      this.appointmentService.deleteAppointment(id).subscribe({
        next: () => {
          // 通过 ID 过滤掉已删除的项，实现 UI 实时同步
          this.appointments.update(list => list.filter(a => a.id !== id));
        }
      });
    }
  }

  onSlotClick(hour: number) {
    console.log(`Booking for ${hour}:00`);
  }

  loadAllData() {
    this.isLoading.set(true);
    this.receptionistService.getPatients().subscribe(data => this.patients.set(data));
    this.receptionistService.getDoctors().subscribe(data => this.doctors.set(data));
    this.receptionistService.getAppointmentsByDate(this.today).subscribe({
      next: (data) => {
        this.todayAppointments.set(data);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
    this.isLoading.set(false);
  }

  private scheduleLoadData(dateStr: string) {
    this.isLoading.set(true);
    this.receptionistService.getAppointmentsByDate(dateStr).subscribe({
      next: (data) => {
        
        this.appointments.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Fetch failed', err);
        this.appointments.set([]);
        this.isLoading.set(false);
      }
    });
  }

  loadPatients() {
    this.receptionistService.getPatients().subscribe(data => this.patients.set(data));
  }

  onSubmitPatient() {
    if (this.patientForm.invalid) return;

    const formData = this.patientForm.getRawValue();
    const newPatient: any = {
      ...formData,
      birthDate: new Date(formData.birthDate)
    };

    this.receptionistService.createPatient(newPatient).subscribe({
      next: () => {
        alert('Patient Registered Successfully');
        this.loadPatients();
        this.setSection('patients');
        this.patientForm.reset();
      },
      error: (err) => console.error('Creation failed', err)
    });
  }

  onSubmitAppointment() {
    if (this.appointmentForm.valid) {
      this.receptionistService.createAppointment(this.appointmentForm.value).subscribe(() => {
        alert('Appointment Booked Successfully');
        this.setSection('queue');
        this.appointmentForm.reset();
      });
    }
  }

  onCheckIn(appointmentId: string) {
    if (confirm('Confirm patient arrival and start clinical visit?')) {
      this.receptionistService.checkIn(appointmentId).subscribe({
        next: (response) => {
          alert('Patient checked in successfully. Clinical record created.');
          this.loadAllData();
        },
        error: (err) => alert('Check-in failed: ' + err.message)
      });
    }
  }

  onNoShow(appointmentId: string) {
    if (confirm('Mark this appointment as No-Show?')) {
      this.receptionistService.markNoShow(appointmentId).subscribe({
        next: () => {
          this.loadAllData();
        }
      });
    }
  }

  setSection(section: string) {
    this.activeSection.set(section);
  }

  getStatusBadgeClass(status: any): string {
    switch (status) {
      case 'Scheduled': return 'badge-info';
      case 'Confirmed': return 'badge-success';
      case 'CheckedIn': return 'badge-primary';
      case 'Cancelled': return 'badge-error';
      case 'Completed': return 'badge-neutral';
      case 'Noshow': return 'badge-ghost';
      default: return 'badge-ghost';
    }
  }
}
