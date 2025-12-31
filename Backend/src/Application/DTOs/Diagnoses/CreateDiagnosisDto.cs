namespace DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses
{
    public class CreateDiagnosisDto
    {
        public Guid VisitId { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string Status { get; set; } = "Confirmed";
        public string? Summary { get; set; }

        public List<CreateDiagnosisItemDto> Items { get; set; } = [];
    }

    public class CreateDiagnosisItemDto
    {
        public string ToothPosition { get; set; } = null!;
        public string DiseaseName { get; set; } = null!;
        public string Severity { get; set; } = "Moderate";
        public string? Notes { get; set; }
    }
}
