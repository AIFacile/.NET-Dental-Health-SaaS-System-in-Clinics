import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';
import { ReceptionistService } from '../../../core/services/receptionist-service';
import { Patient } from '../../../types/patient-types';
import { AppointmentDto } from '../../../types/appointment-types';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Doctor } from '../../../types/user-types';

@Component({
  selector: 'app-receptionist-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './receptionist-dashboard.component.html'
})
export class ReceptionistDashboardComponent implements OnInit {
  public auth = inject(AuthService);
  private receptionistService = inject(ReceptionistService);
  private fb = inject(FormBuilder)

  activeSection = signal<string>('queue');
  patients = signal<Patient[]>([]);
  doctors = signal<Doctor[]>([]);
  appointments = signal<AppointmentDto[]>([]);
  isLoading = signal<boolean>(false);

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
    status: [0],
    visitId: [null]
  });

  ngOnInit() {
    this.loadAllData();
  }

  loadAllData() {
    this.isLoading.set(true);
    this.receptionistService.getPatients().subscribe(data => this.patients.set(data));
    this.receptionistService.getDoctors().subscribe(data => this.doctors.set(data));
    this.receptionistService.getAppointments().subscribe({
      next: (data) => {
        this.appointments.set(data);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
    this.isLoading.set(false);
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

  setSection(section: string) {
    this.activeSection.set(section);
  }
}
