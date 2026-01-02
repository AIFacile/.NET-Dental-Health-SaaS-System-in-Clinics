using DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    [Route("/api/appointments")]
    public class AppointmentsController(IAppointmentService service) : ControllerBase
    {
        private readonly IAppointmentService _service = service;

        // POST /api/appointments
        [HttpPost("/api/appointments")]
        public async Task<ActionResult<AppointmentDto>> Create(
            CreateAppointmentDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
        }

        // GET /api/doctors/{doctorId}/appointments
        [HttpGet("/api/doctors/{doctorId:guid}/appointments")]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByDoctor(Guid doctorId)
            => Ok(await _service.GetByDoctorAsync(doctorId));

        // GET /api/patients/{patientId}/appointments
        [HttpGet("/api/patients/{patientId:guid}/appointments")]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByPatient(Guid patientId)
            => Ok(await _service.GetByPatientAsync(patientId));

        // PUT /api/appointments/{id}
        [HttpPut("/api/appointments/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateAppointmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // POST /api/appointments/{id}/check-in
        [HttpPost("/api/appointments/{id:guid}/check-in")]
        public async Task<ActionResult<VisitDto>> CheckIn(Guid id)
            => Ok(await _service.CheckInAsync(id));

        // POST /api/appointments/{id}/cancel
        [HttpPost("/api/appointments/{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return Ok();
        }

        // POST /api/appointments/{id}/confirm
        [HttpPost("/api/appointments/{id:guid}/confirm")]
        public async Task<ActionResult<VisitDto>> Confirm(Guid id)
        {
            await _service.ConfirmAsync(id);
            return Ok();
        }

        // POST /api/appointments/{id}/No-show
        [HttpPost("/api/appointments/{id:guid}/no-show")]
        public async Task<ActionResult<VisitDto>> NoShow(Guid id)
        {
            await _service.NoShowAsync(id);
            return Ok();
        }
    }
}
