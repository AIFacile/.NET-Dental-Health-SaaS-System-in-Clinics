using DentalHealthSaaS.Backend.src.Domain.Appointments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public Guid? VisitId { get; set; }
    }
}
