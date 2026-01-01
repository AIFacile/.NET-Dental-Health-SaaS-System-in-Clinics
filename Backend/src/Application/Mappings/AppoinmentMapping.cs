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
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                VisitId = appointment.VisitId,
            };
        }
    }
}
