using DentalHealthSaaS.Backend.src.Application.Abstractions.Visits;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    [Route("/api/visits")]
    public class VisitsController(IVisitService service) : ControllerBase
    {
        private readonly IVisitService _service = service;

        [HttpPost]
        public async Task<ActionResult<VisitDto>> Create(
            CreateVisitDto dto)
        {
            await _service.CreateAsync(dto);

            return NoContent();
        }

        // GET /api/visits/{id}
        [HttpGet("/{id:guid}")]
        public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetById(Guid id)
            => Ok(await _service.GetById(id));

        // GET /api/patients/{patientId}/visits
        [HttpGet("/api/patients/{patientId:guid}/visits")]
        public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetByPatientsId(Guid patientId)
            => Ok(await _service.GetByPatientsAsync(patientId));

        // POST /api/visits/{id}/start-diagnosis
        [HttpPost("/{id:guid}/start-diagnosis")]
        public async Task<IActionResult> StartDiagnosis(Guid id)
        {
            await _service.StartDiagnosisAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/start-treatment
        [HttpPost("/{id:guid}/start-treatment")]
        public async Task<IActionResult> StartTreatment(Guid id)
        {
            await _service.StartTreatmentAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/complete
        [HttpPost("/{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _service.CompleteAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/complete
        [HttpPost("/{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return Ok();
        }
    }
}
