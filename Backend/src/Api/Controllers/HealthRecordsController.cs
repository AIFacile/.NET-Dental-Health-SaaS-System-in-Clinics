using DentalHealthSaaS.Backend.src.Application.Abstractions.HealthRecords;
using DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    public class HealthRecordsController (IHealthRecordService service) : ControllerBase
    {
        private readonly IHealthRecordService _service = service;

        [HttpPost("api/patients/{patientId:guid}/health-records")]
        public async Task<ActionResult<HealthRecordDto>> Create(Guid patientId, CreateHealthRecordDto dto)
        {
            var result = await _service.CreateAsync(patientId, dto);
            return Ok(result);
        }

        [HttpGet("api/visits/{visitId:guid}/health-records")]
        public async Task<ActionResult<IReadOnlyList<HealthRecordDto>>> GetByVisit(Guid visitId)
            => Ok(await _service.GetByVisitAsync(visitId));

        [HttpGet("api/patients/{patientId:guid}/health-records")]
        public async Task<ActionResult<IReadOnlyList<HealthRecordDto>>> GetByPatient(Guid patientId)
            => Ok(await _service.GetByVisitAsync(patientId));

        [HttpPut("api/health-records/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateHealthRecordDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}
