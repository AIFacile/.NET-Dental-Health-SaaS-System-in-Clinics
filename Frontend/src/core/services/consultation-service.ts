import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { DiagnosisDto, HealthRecordDto, TreatmentPlanDto, VisitDto } from "../../types/consultation-types";
import { Patient } from "../../types/patient-types";

@Injectable({ providedIn: 'root' })
export class ConsultationService {
  private http = inject(HttpClient);
  private readonly API_BASE = 'https://localhost:7168/api';

  getPatient(patientId: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.API_BASE}/patients/${patientId}`);
  }

  getVisits(patientId: string): Observable<VisitDto[]> {
    return this.http.get<VisitDto[]>(`${this.API_BASE}/patients/${patientId}/visits`);
  }

  startDiagnosis(visitId: string): Observable<void> {
    return this.http.post<void>(`${this.API_BASE}/visits/${visitId}/start-diagnosis`, {});
  }

  startTreatment(visitId: string): Observable<void> {
    return this.http.post<void>(`${this.API_BASE}/visits/${visitId}/start-treatment`, {});
  }

  // --- Diagnosis ---
  getDiagnosis(patientId: string): Observable<DiagnosisDto[]> {
    return this.http.get<DiagnosisDto[]>(`${this.API_BASE}/patients/${patientId}/diagnoses`);
  }
  
  saveDiagnosis(patientId: string, data: Partial<DiagnosisDto>): Observable<DiagnosisDto> {
    return this.http.post<DiagnosisDto>(`${this.API_BASE}/patients/${patientId}/diagnoses`, data);
  }

  // --- Health Record ---
  getHealthRecords(patientId: string): Observable<HealthRecordDto[]> {
    return this.http.get<HealthRecordDto[]>(`${this.API_BASE}/patients/${patientId}/health-records`);
  }

  saveHealthRecord(patientId: string, data: Partial<HealthRecordDto>): Observable<HealthRecordDto> {
    return this.http.post<HealthRecordDto>(`${this.API_BASE}/patients/${patientId}/health-records`, data);
  }

  // --- Treatment Plan ---
  getTreatmentPlans(visitId: string): Observable<TreatmentPlanDto[]> {
    return this.http.get<TreatmentPlanDto[]>(`${this.API_BASE}/visits/${visitId}/treatment-plans`);
  }

  createTreatmentPlan(patientId: string, data: Partial<TreatmentPlanDto>): Observable<TreatmentPlanDto> {
    return this.http.post<TreatmentPlanDto>(`${this.API_BASE}/patients/${patientId}/treatment-plans`, data);
  }

  // --- X-Ray (Placeholder) ---
  uploadXray(visitId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.API_BASE}/visits/${visitId}/xrays`, formData);
  }
}