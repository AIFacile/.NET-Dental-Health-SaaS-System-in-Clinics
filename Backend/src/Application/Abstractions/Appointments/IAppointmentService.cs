using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments
{
    public interface IAppointmentService
    {
        Task<IReadOnlyList<AppointmentDto>> GetByPatientAsync(Guid patientId);
        Task<IReadOnlyList<AppointmentDto>> GetByDoctorAsync(Guid doctorId);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task UpdateAsync(Guid id, UpdateAppointmentDto dto);
        Task CancelAsync(Guid id);
        Task<VisitDto> CheckInAsync(Guid appointmentId);
        Task ConfirmAsync(Guid appointmentId);
        Task NoShowAsync(Guid appointmentId);
    }
}
