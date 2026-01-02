using DentalHealthSaaS.Backend.src.Application.DTOs.Payments;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
        Task<IReadOnlyList<PaymentDto>> GetByVisitAsync(Guid visitId);
        Task<PaymentDto> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid id, UpdatePaymentDto dto);
        Task RefundAsync(Guid id);
    }
}
