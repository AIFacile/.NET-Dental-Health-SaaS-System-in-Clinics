using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.Payments
{
    public class Payment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public DateTime PaidAt { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;
    }
}
