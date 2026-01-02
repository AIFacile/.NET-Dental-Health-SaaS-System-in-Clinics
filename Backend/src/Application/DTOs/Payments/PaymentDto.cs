using DentalHealthSaaS.Backend.src.Domain.Payments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Payments
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid VisitId { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public DateTime PaidAt { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
