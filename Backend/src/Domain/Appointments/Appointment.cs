using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Users;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.Appointments
{
    public class Appointment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public Guid? VisitId { get; set; }
        public Visit? Visit { get; set; }
    }

}
