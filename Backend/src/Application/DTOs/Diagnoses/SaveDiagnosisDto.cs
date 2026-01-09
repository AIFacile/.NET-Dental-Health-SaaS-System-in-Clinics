using DentalHealthSaaS.Backend.src.Domain.Diagnoses;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses
{
    public class SaveDiagnosisDto
    {
        public Guid VisitId { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public DiagnosisStatus Status { get; set; } = DiagnosisStatus.Draft;
        public string? Summary { get; set; }

        public List<DiagnosisItemDto> Items { get; set; } = [];
    }
}
