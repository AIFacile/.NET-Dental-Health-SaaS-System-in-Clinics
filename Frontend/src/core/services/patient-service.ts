import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Patient } from '../../types/patient-types';


@Injectable({ providedIn: 'root' })
export class PatientService {
  private http = inject(HttpClient);
  private readonly API_URL = 'https://localhost:7168/api/patients';

  getPatients(): Observable<Patient[]> {
    // Mock data for initial UI building
    const mockPatients: Patient[] = [
      { id: '1', name: 'John Doe', age: 45, gender: 'Male', lastVisit: '2026-01-02', status: 'Under Treatment', phone: '+1 234-567-890' },
      { id: '2', name: 'Jane Smith', age: 32, gender: 'Female', lastVisit: '2025-12-28', status: 'Healthy', phone: '+1 987-654-321' },
      { id: '3', name: 'Robert Brown', age: 60, gender: 'Male', lastVisit: '2026-01-04', status: 'Emergency', phone: '+1 555-019-222' },
    ];
    return of(mockPatients); 
    // Later replace with: return this.http.get<Patient[]>(this.API_URL);
  }

  getPatientById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.API_URL}/${id}`);
  }
}