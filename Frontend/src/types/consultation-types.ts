// Enums
export enum DiagnosisStatus { Confirmed = 0, Tentative = 1, Refuted = 2 }
export enum TreatmentPlanStatus { Draft = 0, Active = 1, Completed = 2 }
export enum TreatmentStepStatus { Pending = 0, InProgress = 1, Completed = 2 }

// DTOs
export interface VisitDto {
  id?: string;
  visitDate: string;
  visitType: string;
  status: string;
  patientId: string;
  DoctorId: string;
}

export interface DiagnosisItemDto {
  id?: string;
  toothPosition: string;
  diseaseName: string;
  severity: string;
  notes?: string;
}

export interface DiagnosisDto {
  id: string;
  visitId: string;
  patientId: string;
  diagnosisDate: string;
  status: string;
  summary?: string;
  items: DiagnosisItemDto[];
}

export interface HealthRecordDto {
  id: string;
  visitId: string;
  patientId: string;
  toothPosition: string;
  dentalStatus: string;
  notes?: string;
  recordedAt: string;
}

export interface TreatmentStepDto {
  id?: string; // Optional for creation
  stepOrder: number;
  description: string;
  cost: number;
  status: TreatmentStepStatus;
}

export interface TreatmentPlanDto {
  id: string;
  visitId: string;
  patientId: string;
  planType: string;
  estimatedCost: number;
  status: TreatmentPlanStatus;
  steps: TreatmentStepDto[];
}