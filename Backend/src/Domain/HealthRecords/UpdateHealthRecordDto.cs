namespace DentalHealthSaaS.Backend.src.Domain.HealthRecords
{
    public class UpdateHealthRecordDto
    {
        public string DentalSatus { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
