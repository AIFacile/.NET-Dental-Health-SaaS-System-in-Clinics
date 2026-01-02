namespace DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans
{
    public class CreateTreatmentPlanDto
    {
        public Guid VisitId { get; set; }

        public string PlanType { get; set; } = null!;

        public IReadOnlyList<CreateTreatmentStepDto> Steps { get; set; } = [];
    }
}
