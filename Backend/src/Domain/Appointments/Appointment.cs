using DentalHealthSaaS.Backend.src.Domain.Common;

namespace DentalHealthSaaS.Backend.src.Domain.Appointments
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; } = "Scheduled";
    }
}
