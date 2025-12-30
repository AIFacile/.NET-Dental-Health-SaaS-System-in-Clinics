namespace DentalHealthSaaS.Backend.src.Domain.Diagnoses
{
    public class DiagnosisItem
    {
        public Guid Id { get; set; }

        public Guid DiagnosisId { get; set; }
        public Diagnosis Diagnosis { get; set; } = null!;

        public required string ToothPosition { get; set; }
        public required string DiseaseName { get; set; }

        public string Severity { get; set; } = "Moderate";
        public string? Notes { get; set; }
    }
}
