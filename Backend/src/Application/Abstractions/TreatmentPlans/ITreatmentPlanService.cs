using DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.TreatmentPlans
{
    public interface ITreatmentPlanService
    {
        Task<TreatmentPlanDto> CreateAsync(Guid patientId, CreateTreatmentPlanDto dto);
        Task<TreatmentPlanDto> GetByIdAsync(Guid id);
        Task<IReadOnlyList<TreatmentPlanDto>> GetByVisitAsync(Guid visitId);
        Task UpdateAsync(Guid id, UpdateTreatmentPlanDto dto);
    }
}
