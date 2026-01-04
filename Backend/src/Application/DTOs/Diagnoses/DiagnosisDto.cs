using DentalHealthSaaS.Backend.src.Domain.Diagnoses;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses
{
    public class DiagnosisDto
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public DiagnosisStatus Status { get; set; } = DiagnosisStatus.Confirmed;
        public string? Summary { get; set; }

        public IReadOnlyList<DiagnosisItemDto> Items { get; set; } = [];
    }
}
