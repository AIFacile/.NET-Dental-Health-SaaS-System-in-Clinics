using DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.HealthRecords
{
    public interface IHealthRecordService
    {
        Task<HealthRecordDto> CreateAsync(Guid patientId, CreateHealthRecordDto dto);
        Task<IReadOnlyList<HealthRecordDto>> GetByVisitAsync(Guid visitId);
        Task<IReadOnlyList<HealthRecordDto>> GetByPatietnAsync(Guid patientId);
        Task UpdateAsync(Guid id, UpdateHealthRecordDto dto);
    }
}
