using DentalHealthSaaS.Backend.src.Application.DTOs.Patients;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Patients
{
    public interface IPatientService
    {
        Task<IReadOnlyList<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(Guid id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task UpdateAsync(Guid id, UpdatePatientDto dto);
        Task DeleteAsync(Guid id);
    }
}
