using DentalHealthSaaS.Backend.src.Domain.Appointments;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = null!;
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public Guid? VisitId { get; set; }
        public Visit? Visit { get; set; }
    }
}
