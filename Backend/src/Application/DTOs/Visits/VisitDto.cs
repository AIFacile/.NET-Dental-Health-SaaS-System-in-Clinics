using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Visits
{
    public class VisitDto
    {
        public Guid Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitType { get; set; } = "Initial";
        public VisitStatus Status { get; set; } = VisitStatus.Open;

        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        public string? Notes { get; set; }
    }
}
