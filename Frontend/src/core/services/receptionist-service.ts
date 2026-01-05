import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Patient } from '../../types/patient-types';
import { AppointmentDto } from '../../types/appointment-types';
import { Doctor } from '../../types/user-types';

@Injectable({
  providedIn: 'root',
})
export class ReceptionistService {
  private http = inject(HttpClient);
  private readonly API_BASE = 'https://localhost:7168/api';

  // Patient APIs
  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${this.API_BASE}/patients`);
  }

  getDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(`${this.API_BASE}/doctors`);
  }
  
  createPatient(patient: Omit<Patient, 'id'>): Observable<Patient> {
    return this.http.post<Patient>(`${this.API_BASE}/patients`, patient);
  }

  deletePatient(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_BASE}/patients/${id}`);
  }

  // Appointment APIs
  getAppointments(): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(`${this.API_BASE}/appointments/today`)
  }

  checkIn(id: string): Observable<void> {
    return this.http.post<void>(`${this.API_BASE}/appointments/${id}/check-in`, {});
  }

  cancelAppointment(id: string): Observable<void> {
    return this.http.post<void>(`${this.API_BASE}/appointments/${id}/cancel`, {});
  }

  
  createAppointment(appointment: any): Observable<any> {
    return this.http.post(`${this.API_BASE}/appointments`, appointment);
  }
}
