namespace DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords
{
    public class HealthRecordDto
    {
        public Guid Id { get; set; }

        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }

        public string ToothPosition { get; set; } = null!;
        public string DentalStatus { get; set; } = null!;

        public string? Notes { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
