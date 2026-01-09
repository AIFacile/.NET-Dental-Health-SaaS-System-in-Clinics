using DentalHealthSaaS.Backend.src.Application.Abstractions.Patients;
using DentalHealthSaaS.Backend.src.Application.DTOs.Patients;
using DentalHealthSaaS.Backend.src.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    //POST   /api/patients
    //GET    /api/patients
    //GET    /api/patients/{id}
    //PUT    /api/patients/{id}
    //DELETE /api/patients/{id}

    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController(IPatientService service) : ControllerBase
    {
        private readonly IPatientService _service = service;

        [Authorize(Policy = Permissions.Patients_Read)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetAll() 
            => Ok(await _service.GetAllAsync());

        [Authorize(Policy = Permissions.Patients_Read)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [Authorize(Policy = Permissions.Patients_Write)]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientDto dto)
            => Ok(await _service.CreateAsync(dto));

        [Authorize(Policy = Permissions.Patients_Write)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdatePatientDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [Authorize(Policy = Permissions.Patients_Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
