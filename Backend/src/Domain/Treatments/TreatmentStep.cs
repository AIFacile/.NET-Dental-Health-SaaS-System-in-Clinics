namespace DentalHealthSaaS.Backend.src.Domain.Treatments
{
    public class TreatmentStep
    {
        public Guid Id { get; set; }

        public Guid TreatmentPlanId { get; set; }
        public TreatmentPlan TreatmentPlan { get; set; } = null!;

        public int StepOrder { get; set; }
        public required string Description { get; set; }

        public decimal Cost { get; set; }
        public TreatmentStepStatus Status { get; set; } = TreatmentStepStatus.Pending;
    }
}
