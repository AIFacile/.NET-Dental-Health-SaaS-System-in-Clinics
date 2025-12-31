using DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Diagnoses
{
    public interface IDiagnosisService
    {
        Task<IReadOnlyList<DiagnosisDto>> GetByPatientAsync(Guid patientId);
        Task<DiagnosisDto> GetByIdAsync(Guid id);
        Task<DiagnosisDto> CreateAsync(
            Guid patientId,
            CreateDiagnosisDto dto);
        Task UpdateAsync(Guid id, UpdateDiagnosisDto dto);
    }
}
