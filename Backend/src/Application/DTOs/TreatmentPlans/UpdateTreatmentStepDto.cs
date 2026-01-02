using DentalHealthSaaS.Backend.src.Domain.Treatments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans
{
    public class UpdateTreatmentStepDto
    {
        public Guid? Id { get; set; }

        public int StepOrder { get; set; }
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        public TreatmentStepStatus Status { get; set; } = TreatmentStepStatus.Pending;

    }
}
