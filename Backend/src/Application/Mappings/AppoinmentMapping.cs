using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Domain.Appointments;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class AppoinmentMapping
    {
        public static AppointmentDto ToDto(this Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.Name,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.RealName,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                VisitId = appointment.VisitId,
                Visit = appointment.Visit,
            };
        }
    }
}
