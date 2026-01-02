using DentalHealthSaaS.Backend.src.Domain.Treatments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans
{
    public class TreatmentPlanDto
    {
        public Guid Id { get; set; }

        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }

        public string PlanType { get; set; } = null!;
        public decimal EstimatedCost { get; set; }
        public TreatmentPlanStatus Status { get; set; } = TreatmentPlanStatus.Draft;

        public IReadOnlyList<TreatmentStepDto> Steps { get; set; } = [];
    }
}
