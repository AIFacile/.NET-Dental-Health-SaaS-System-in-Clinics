using DentalHealthSaaS.Backend.src.Application.Abstractions.Payments;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.DTOs.Payments;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.Payments;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class PaymentService (ApplicationDbContext db, IUserContext user) : IPaymentService
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IUserContext _user = user;

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var visit = await _db.Visits
                .FirstOrDefaultAsync(v => v.Id == dto.VisitId)
                ?? throw new Exception("Visit not found.");

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PatientId = visit.PatientId,
                VisitId = visit.Id,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaidAt = DateTime.UtcNow,
                Status = PaymentStatus.Paid
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return payment.ToDto();
        }

        public async Task<PaymentDto> GetByIdAsync(Guid id)
        {
            var payment = await _db.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Payment not found.");

            return payment.ToDto();
        }

        public async Task<IReadOnlyList<PaymentDto>> GetByVisitAsync(Guid visitId)
        {
            return await _db.Payments
                .Where(p => p.VisitId == visitId)
                .AsNoTracking()
                .Select(p => p.ToDto())
                .ToListAsync();
        }

        public async Task RefundAsync(Guid id)
        {
            var payment = await _db.Payments
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Payment not found.");

            if (payment.Status != PaymentStatus.Paid)
                throw new Exception("Only paid payments can be refunded.");

            payment.Status = PaymentStatus.Refunded;
            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid id, UpdatePaymentDto dto)
        {
            var payment = await _db.Payments
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Payment not found.");

            if (payment.Status == PaymentStatus.Cancelled)
                throw new Exception("Cannot modify cancelled payment.");

            payment.Status = dto.Status;
            await _db.SaveChangesAsync();
        }
    }
}
