using DentalHealthSaaS.Backend.src.Application.DTOs.Payments;
using DentalHealthSaaS.Backend.src.Domain.Payments;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class PaymentMapping
    {
        public static PaymentDto ToDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                PatientId = payment.PatientId,
                VisitId = payment.VisitId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaidAt = payment.PaidAt,
                Status = payment.Status
            };
        }
    }
}
