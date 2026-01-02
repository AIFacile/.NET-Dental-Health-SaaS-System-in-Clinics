using DentalHealthSaaS.Backend.src.Domain.Treatments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans
{
    public class UpdateTreatmentPlanDto
    {
        public TreatmentPlanStatus Status { get; set; } = TreatmentPlanStatus.Draft;

        public IReadOnlyList<UpdateTreatmentStepDto> Steps { get; set; } = [];
    }
}
