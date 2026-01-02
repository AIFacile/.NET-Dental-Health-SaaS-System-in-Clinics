namespace DentalHealthSaaS.Backend.src.Domain.HealthRecords
{
    public class CreateHealthRecordDto
    {
        public Guid VisitId { get; set; }

        public string ToothPosition { get; set; } = null!;
        public string DentalStatus { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
