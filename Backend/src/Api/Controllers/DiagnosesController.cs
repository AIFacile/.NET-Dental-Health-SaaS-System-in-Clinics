using DentalHealthSaaS.Backend.src.Application.Abstractions.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    // POST  /api/patients/{patientId}/diagnoses
    // GET   /api/patients/{patientId}/diagnoses
    // GET   /api/diagnoses/{id}
    // PUT   /api/diagnoses/{id}
    [ApiController]
    [Authorize]
    public class DiagnosesController(IDiagnosisService service) : ControllerBase
    {
        private readonly IDiagnosisService _service = service;

        [Authorize(Policy = Permissions.Diagnoses_Read)]
        [HttpGet("/api/patients/{patientId:guid}/diagnoses")]
        public async Task<ActionResult<IReadOnlyList<DiagnosisDto>>> GetAll(Guid patientId)
            => Ok(await _service.GetByPatientAsync(patientId));

        [Authorize(Policy = Permissions.Diagnoses_Read)]
        [HttpGet("/api/diagnoses/{id:guid}")]
        public async Task<ActionResult<DiagnosisDto>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Policy = Permissions.Diagnoses_Write)]
        [HttpPost("/api/patients/{patientId}/diagnoses")]
        public async Task<IActionResult> Create(Guid patientId, CreateDiagnosisDto dto)
        {
            await _service.CreateAsync(patientId, dto);

            return NoContent();
        }

        [Authorize(Policy = Permissions.Diagnoses_Write)]
        [HttpPut("/api/diagnoses/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateDiagnosisDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [Authorize(Policy = Permissions.Diagnoses_Write)]
        [HttpPost("/api/patients/{patientId:guid}/diagnoses")]
        public async Task<ActionResult<DiagnosisDto>> Upsert(Guid patientId, SaveDiagnosisDto dto)
        {
            await _service.UpsertAsync(patientId, dto);
            return NoContent();
        }
    }
}
