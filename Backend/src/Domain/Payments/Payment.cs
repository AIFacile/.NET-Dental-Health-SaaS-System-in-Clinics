using DentalHealthSaaS.Backend.src.Domain.Common;

namespace DentalHealthSaaS.Backend.src.Domain.Payments
{
    public class Payment : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Guid VisitId { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public DateTime PaidAt { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;
    }
}
