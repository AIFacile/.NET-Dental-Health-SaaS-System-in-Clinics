using DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses;
using DentalHealthSaaS.Backend.src.Domain.Diagnoses;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class DiagnosisMapping
    {
        public static DiagnosisDto ToDto(this Diagnosis diagnosis)
        {
            return new DiagnosisDto
            {
                Id = diagnosis.Id,
                VisitId = diagnosis.VisitId,
                PatientId = diagnosis.PatientId,
                DoctorId = diagnosis.DoctorId,
                DiagnosisDate = diagnosis.DiagnosisDate,
                Status = diagnosis.Status,
                Summary = diagnosis.Summary,
                Items = [.. diagnosis.Items.Select(i => new DiagnosisItemDto
                {
                    Id = i.Id,
                    ToothPosition = i.ToothPosition,
                    DiseaseName = i.DiseaseName,
                    Severity = i.Severity,
                    Notes = i.Notes,
                })],
            };
        }

        public static Diagnosis ToEntity(this CreateDiagnosisDto dto, Guid patientId, Guid userId)
        {
            return new Diagnosis
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                VisitId = dto.VisitId,
                DiagnosisDate = dto.DiagnosisDate,
                DoctorId = userId,
                Summary = dto.Summary,
                Items = [.. dto.Items.Select(i => new DiagnosisItem
                {
                    Id = new Guid(),
                    ToothPosition = i.ToothPosition,
                    DiseaseName = i.DiseaseName,
                    Severity = i.Severity,
                    Notes = i.Notes,
                })]
            };
        }
    }
}
