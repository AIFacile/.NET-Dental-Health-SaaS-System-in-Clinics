using DentalHealthSaaS.Backend.src.Domain.Payments;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Payments
{
    public class UpdatePaymentDto
    {
        public PaymentStatus Status { get; set; }
    }
}
