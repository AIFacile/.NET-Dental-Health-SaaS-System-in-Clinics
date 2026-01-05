import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

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

  // // Helper to get text labels for status
  // getStatusLabel(status: any): string {
  //   // 将 status 强制转换为数字。即使后端传回的是字符串 "0"，Number("0") 也会变成数字 0
  //   const statusKey = Number(status) as AppointmentStatus;
  
  //   const statusMap: Record<number, string> = {
  //     [AppointmentStatus.Scheduled]: 'Scheduled',
  //     [AppointmentStatus.Confirmed]: 'Confirmed',
  //     [AppointmentStatus.CheckedIn]: 'Checked-In',
  //     [AppointmentStatus.Cancelled]: 'Cancelled',
  //     [AppointmentStatus.Completed]: 'Completed',
  //     [AppointmentStatus.Noshow]: 'No Show'
  //   };
  
  //   return statusMap[statusKey] || `Unknown (${status})`;
  // }

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