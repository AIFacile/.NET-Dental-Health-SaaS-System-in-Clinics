using DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans;
using DentalHealthSaaS.Backend.src.Domain.Diagnoses;
using DentalHealthSaaS.Backend.src.Domain.Treatments;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class TreatmentPlanMapping
    {
        public static TreatmentPlanDto ToDto(this TreatmentPlan entity)
        {
            return new TreatmentPlanDto
            {
                Id = entity.Id,
                VisitId = entity.VisitId,
                PatientId = entity.PatientId,
                PlanType = entity.PlanType,
                EstimatedCost = entity.EstimatedCost,
                Status = entity.Status,
                Steps = [.. entity.Steps.Select(i => new TreatmentStepDto
                {
                    Id = i.Id,
                    StepOrder = i.StepOrder,
                    Description = i.Description,
                    Cost = i.Cost,
                    Status = i.Status,
                })],
            };
        }
    }
}
