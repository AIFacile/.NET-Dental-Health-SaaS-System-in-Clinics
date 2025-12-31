namespace DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses
{
    public class DiagnosisItemDto
    {
        /// <summary>
        /// null = New item
        /// not null = Existing item
        /// </summary>
        public Guid? Id { get; set; }

        public string ToothPosition { get; set; } = null!;
        public string DiseaseName { get; set; } = null!;
        public string Severity { get; set; } = "Moderate";
        public string? Notes { get; set; }
    }
}
