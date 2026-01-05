export interface AppointmentDto {
  patientId: string;
  patientName?: string;
  doctorId: string;
  startTime: string;
  endTime: string;
  status: string;
  visitId?: string;
}
