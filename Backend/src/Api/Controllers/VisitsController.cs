using DentalHealthSaaS.Backend.src.Application.Abstractions.Visits;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    public class VisitsController(IVisitService service) : ControllerBase
    {
        private readonly IVisitService _service = service;

        // ********************** SuperAdmin Only ********************** //
        [HttpPost("/api/visits")]
        public async Task<ActionResult<VisitDto>> Create(
            CreateVisitDto dto)
        {
            await _service.CreateAsync(dto);

            return NoContent();
        }

        // GET /api/visits/{id}
        [HttpGet("/api/visits/{id:guid}")]
        public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetById(Guid id)
            => Ok(await _service.GetById(id));

        // GET /api/patients/{patientId}/visits
        [HttpGet("/api/patients/{patientId:guid}/visits")]
        public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetByPatientsId(Guid patientId)
            => Ok(await _service.GetByPatientsAsync(patientId));
        // ************************************************************* //

        // ------------------------- Doctors --------------------------- //
        // GET /api/me/appointments/{patientId:guid}/visits
        // This API is for doctors to inspect a patient's visits.
        // The root page should be doctor's appointment list.
        [HttpGet("/api/me/appointments/{patientId:guid}/visits")]
        public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetByPatientIdInAppointment(Guid patientId)
            => Ok(await _service.GetByPatientsAsync(patientId));

        // POST /api/visits/{id}/start-diagnosis
        [HttpPost("/api/visits/{id:guid}/start-diagnosis")]
        public async Task<IActionResult> StartDiagnosis(Guid id)
        {
            await _service.StartDiagnosisAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/start-treatment
        [HttpPost("/api/visits/{id:guid}/start-treatment")]
        public async Task<IActionResult> StartTreatment(Guid id)
        {
            await _service.StartTreatmentAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/complete
        [HttpPost("/api/visits/{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _service.CompleteAsync(id);
            return Ok();
        }

        // POST /api/visits/{id}/cancel
        [HttpPost("/api/visits/{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return Ok();
        }
        // ------------------------------------------------------------- //
    }
}
