import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';
import { AppointmentService } from '../../../core/services/appointment-service';
import { AuthService } from '../../../core/services/auth-service';
import { AppointmentDto } from '../../../types/appointment-types';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './doctor-dashboard.component.html'
})
export class DoctorDashboardComponent implements OnInit {
  public auth = inject(AuthService);
  public router = inject(Router);
  private appointmentService = inject(AppointmentService);

  activeSection = signal<string>('today');
  todayAppointments = signal<AppointmentDto[]>([]);
  allAppointments = signal<AppointmentDto[]>([]);
  isLoading = signal<boolean>(false);

  ngOnInit() {
    this.loadAllData();
  }

  loadAllData() {
    this.isLoading.set(true);
    // Fetch both for a seamless experience
    this.appointmentService.getTodayAppointments().subscribe(data => this.todayAppointments.set(data));
    this.appointmentService.getAllAppointments().subscribe({
      next: (data) => {
        this.allAppointments.set(data);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  onConfirm(appointmentId: string) {
    if (confirm('Make sure you have availability to confirm patient arrival.')) {
      this.appointmentService.confirm(appointmentId).subscribe({
        next: (response) => {
          alert('Patient checked in successfully. Clinical record created.');
          this.loadAllData();
        },
        error: (err) => alert('Confirmation failed: ' + err.message)
      });
    }
  }

  startConsultation(patientId: string, visitId?: string) {
    // 如果没有 visitId (尚未 Check-in)，可以先调用 createVisit API，然后再跳转
    if (!visitId) {
       alert("Patient hasn't checked in yet.");
       return;
    }
    this.router.navigate(['/doctor/consultation', patientId, visitId]);
 }

  setSection(section: string) {
    this.activeSection.set(section);
  }

  /**
   * Formats the date to show full date + time range
   * Example: Jan 05, 2026 | 09:00 - 10:00
   */
  formatFullDateTime(start: string, end: string): string {
    const startDate = new Date(start);
    const endDate = new Date(end);
    
    const datePart = startDate.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: '2-digit' 
    });
    
    const timePart = `${startDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })} - ${endDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })}`;
    
    return `${datePart} | ${timePart}`;
  }

  // Helper to format the time slot
  formatTimeRange(start: string, end: string): string {
    const s = new Date(start).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    const e = new Date(end).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    return `${s} - ${e}`;
  }

  // Helper to get CSS classes for status badges
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

  // Calendar State
  viewDate = signal<Date>(new Date());
  daysOfWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  // Computed property to generate days for the current view
  calendarDays = computed(() => {
    const date = this.viewDate();
    const year = date.getFullYear();
    const month = date.getMonth();

    const firstDayOfMonth = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    const days = [];

    // Add empty slots for days of the week before the 1st
    for (let i = 0; i < firstDayOfMonth; i++) {
      days.push({ day: null, fullDate: null });
    }

    // Add actual days of the month
    for (let i = 1; i <= daysInMonth; i++) {
      days.push({
        day: i,
        fullDate: new Date(year, month, i).toISOString().split('T')[0],
        isToday: this.isToday(year, month, i)
      });
    }
    return days;
  });

  setToday(): void {
    this.viewDate.set(new Date());
  }

  isToday(year: number, month: number, day: number): boolean {
    const today = new Date();
    return today.getFullYear() === year &&
      today.getMonth() === month &&
      today.getDate() === day;
  }

  changeMonth(delta: number) {
    const current = this.viewDate();
    this.viewDate.set(new Date(current.getFullYear(), current.getMonth() + delta, 1));
  }
}