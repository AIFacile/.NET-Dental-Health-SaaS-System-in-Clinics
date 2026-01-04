using DentalHealthSaaS.Backend.src.Application.Abstractions.Payments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Payments;
using DentalHealthSaaS.Backend.src.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    // POST /api/payments
    // GET /api/payments/{id}
    // GET /api/visits/{visitId}/payments
    // PUT /api/payments/{id}/status
    // POST /api/payments/{id}/refund

    [ApiController]
    public class PaymentsController (IPaymentService service) : ControllerBase
    {
        private readonly IPaymentService _service = service;

        [Authorize(Policy = Permissions.Payments_Write)]
        [HttpPost("api/payments")]
        public async Task<ActionResult<PaymentDto>> Create(CreatePaymentDto dto)
            => Ok(await _service.CreateAsync(dto));

        [Authorize(Policy = Permissions.Payments_Read)]
        [HttpGet("api/payments/{id:guid}")]
        public async Task<ActionResult<PaymentDto>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Policy = Permissions.Payments_Read)]
        [HttpGet("/api/visits/{visitId:guid}/payments")]
        public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetByVisit(Guid visitId)
            => Ok(await _service.GetByVisitAsync(visitId));

        [Authorize(Policy = Permissions.Payments_Write)]
        [HttpPut("api/payments/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdatePaymentDto dto)
        {
            await _service.UpdateStatusAsync(id, dto);
            return NoContent();
        }

        [Authorize(Policy = Permissions.Payments_Write)]
        [HttpPost("api/payments/{id:guid}/refund")]
        public async Task<IActionResult> Refund(Guid id)
        {
            await _service.RefundAsync(id);
            return NoContent();
        }
    }
}
