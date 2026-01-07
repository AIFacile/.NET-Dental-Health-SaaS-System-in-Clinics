using DentalHealthSaaS.Backend.src.Domain.Appointments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Appointments
{
    public class UpdateAppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    }
}
