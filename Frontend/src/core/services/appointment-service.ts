import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { AppointmentDto } from "../../types/appointment-types";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private http = inject(HttpClient);
  private readonly API_URL = 'https://localhost:7168/api/me/appointments';

  getTodayAppointments(): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(`${this.API_URL}/today`);
  }

  getAllAppointments(): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(this.API_URL);
  }

  confirm(id: string): Observable<void> {
    return this.http.post<void>(`${this.API_URL}/${id}/confirm`, {});
  }

  deleteAppointment(id: string): Observable<void>{
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }

  cancelAppointment(id: string): Observable<void> {
    return this.http.post<void>(`${this.API_URL}/appointments/${id}/cancel`, {});
  }

}