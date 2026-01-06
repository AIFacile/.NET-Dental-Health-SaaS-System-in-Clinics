export interface AppointmentDto {
  id: string;
  patientId: string;
  patientName?: string;
  doctorId: string;
  doctorName?: string;
  startTime: string;
  endTime: string;
  status: string;
  visitId?: string;
}
