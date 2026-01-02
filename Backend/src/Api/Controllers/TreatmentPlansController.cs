using DentalHealthSaaS.Backend.src.Application.Abstractions.TreatmentPlans;
using DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    public class TreatmentPlansController (ITreatmentPlanService service) : ControllerBase
    {
        private readonly ITreatmentPlanService _service = service;

        [HttpPost("api/patients/{patientId:guid}/treatment-plans")]
        public async Task<ActionResult<TreatmentPlanDto>> Create(Guid patientId, CreateTreatmentPlanDto dto)
            => Ok(await _service.CreateAsync(patientId, dto));

        [HttpGet("api/treatment-plans/{id:guid}")]
        public async Task<ActionResult<TreatmentPlanDto>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("api/visits/{visitId:guid}/treatment-plans")]
        public async Task<ActionResult<IReadOnlyList<TreatmentPlanDto>>> GetByVisit(Guid visitId)
            => Ok(await _service.GetByVisitAsync(visitId));

        [HttpPut("api/treatment-plans/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTreatmentPlanDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}
