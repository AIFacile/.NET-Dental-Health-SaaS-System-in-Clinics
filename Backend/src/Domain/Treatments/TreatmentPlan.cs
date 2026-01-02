using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.Treatments
{
    public class TreatmentPlan : BaseEntity
    {
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public Guid PatientId { get; set; }

        public required string PlanType { get; set; }

        public decimal EstimatedCost { get; set; }
        public TreatmentPlanStatus Status { get; set; } = TreatmentPlanStatus.Draft;

        public ICollection<TreatmentStep> Steps { get; set; } = new List<TreatmentStep>();
    }
}
