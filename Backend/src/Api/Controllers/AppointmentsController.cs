using DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using DentalHealthSaaS.Backend.src.Application.Security;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    //[Authorize]
    public class AppointmentsController(IAppointmentService service) : ControllerBase
    {
        private readonly IAppointmentService _service = service;

        // ------------------------- Doctors --------------------------- //
        // GET /api/me/appointments
        // This API is for doctors to see all appointments (including past ones).
        // The root page should be doctor dashboard.
        [HttpGet("/api/me/appointments")]
        [Authorize(Policy = Permissions.Appointments_Read)]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByDoctor()
            => Ok(await _service.GetByDoctorAsync());

        // GET /api/me/appointments/today
        // This API is for doctors to see today's appointments.
        // The root page should be doctor dashboard.
        [HttpGet("/api/me/appointments/today")]
        [Authorize(Policy = Permissions.Appointments_Read)]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByDoctorToday()
            => Ok(await _service.GetByDoctorTodayAsync());

        // POST /api/me/appointments/{id:guid}/confirm
        // This API is for doctors to confirm specific appointments depending on their availability.
        // The root page should be doctor dashboard.
        [HttpPost("/api/me/appointments/{id:guid}/confirm")]
        public async Task<ActionResult<VisitDto>> Confirm(Guid id)
        {
            await _service.ConfirmAsync(id);
            return Ok();
        }

        // POST /api/me/appointments/{id:guid}/complete
        // This API is for doctors to complete specific appointments when patient ready to leave.
        // The root page should be doctor dashboard.
        [HttpPost("/api/me/appointments/{id:guid}/complete")]
        public async Task<ActionResult<VisitDto>> Complete(Guid id)
        {
            await _service.CompleteAsync(id);
            return Ok();
        }
        // ------------------------------------------------------------- //

        // ----------------------- Receptionists ----------------------- //
        // GET /api/patients/{patientId}/appointments
        // This API is for recptionists to see a patient's appointments.
        // The root page should be patient list.
        [HttpGet("/api/patients/{patientId:guid}/appointments")]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByPatient(Guid patientId)
            => Ok(await _service.GetByPatientAsync(patientId));

        // GET /api/patients/{patientId}/appointments
        // This API is for recptionists to see a patient's appointments.
        // The root page should be patient list.
        //[HttpGet("/api/appointments")]
        //public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAll()
        //    => Ok(await _service.GetAllAsync());

        // GET /api/appointments/{date}
        // This API is for recptionists to see a patient's appointments.
        // The root page should be patient list.
        [HttpGet("/api/appointments")]
        public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetByDate([FromQuery] DateTime date)
            => Ok(await _service.GetByDateAsync(date));

        // POST /api/appointments
        // This API allows receptionists to create appointments.
        // The root page should be appointment list.
        [HttpPost("/api/appointments")]
        public async Task<ActionResult<AppointmentDto>> Create(
            CreateAppointmentDto dto)
        {
            await _service.CreateAsync(dto);
            return NoContent();
        }

        // GET /api/appointments/today
        // This API is for receptionists to get today's appointments.
        // The root page should be appointment list.
        //[HttpGet("/api/appointments/today")]
        //public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetToday()
        //    => Ok(await _service.GetByDateAsync(DateTime.Today));

        // PUT /api/appointments/{id}
        // This API is for receptionists to update appointments.
        // The root page should be appointment list.
        [HttpPut("/api/appointments/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateAppointmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // POST /api/appointments/{id}/check-in
        // This API is for receptionists to check patient in.
        // The root page should be appointment list.
        [HttpPost("/api/appointments/{id:guid}/check-in")]
        public async Task<ActionResult<VisitDto>> CheckIn(Guid id)
            => Ok(await _service.CheckInAsync(id));

        // POST /api/appointments/{id}/cancel
        // This API is for receptionists to cancel appointments.
        // The root page should be appointment list.
        [HttpPost("/api/appointments/{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return Ok();
        }

        // POST /api/appointments/{id}/No-show
        // This API is for receptionists to disactivate appointment when patient didn't show up.
        // The root page should be appointment list.
        [HttpPost("/api/appointments/{id:guid}/no-show")]
        public async Task<ActionResult<VisitDto>> NoShow(Guid id)
        {
            await _service.NoShowAsync(id);
            return Ok();
        }
        // ------------------------------------------------------------- //
    }
}
