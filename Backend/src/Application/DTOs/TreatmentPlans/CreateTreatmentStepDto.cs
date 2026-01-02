namespace DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans
{
    public class CreateTreatmentStepDto
    {
        public int StepOrder { get; set; }
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
