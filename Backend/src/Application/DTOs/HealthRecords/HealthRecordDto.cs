using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords
{
    public class HealthRecordDto
    {
        public Guid Id { get; set; }

        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public string ToothPosition { get; set; } = null!;
        public string DentalStatus { get; set; } = null!;

        public string? Notes { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
