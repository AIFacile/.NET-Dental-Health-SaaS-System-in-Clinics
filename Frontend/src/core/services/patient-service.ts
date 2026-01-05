import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Patient } from '../../types/patient-types';


@Injectable({ providedIn: 'root' })
export class PatientService {
  private http = inject(HttpClient);
  private readonly API_URL = 'https://localhost:7168/api/patients';

  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.API_URL);
  }

  getPatientById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.API_URL}/${id}`);
  }
}