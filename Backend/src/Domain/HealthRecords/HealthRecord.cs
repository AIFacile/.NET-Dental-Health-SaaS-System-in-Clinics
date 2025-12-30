using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.HealthRecords
{
    public class HealthRecord : BaseEntity
    {
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public Guid PatientId { get; set; }

        public required string ToothPosition { get; set; }
        public required string DentalStatus { get; set; }

        public string? Notes { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
