using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments
{
    public interface IAppointmentService
    {
        Task<IReadOnlyList<AppointmentDto>> GetByPatientAsync(Guid patientId);
        Task<IReadOnlyList<AppointmentDto>> GetByDoctorAsync();
        Task<IReadOnlyList<AppointmentDto>> GetByDoctorTodayAsync();
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task UpdateAsync(Guid id, UpdateAppointmentDto dto);
        Task<IReadOnlyList<AppointmentDto>> GetTodayAsync();
        Task CancelAsync(Guid appointmentId);
        Task<VisitDto> CheckInAsync(Guid appointmentId);
        Task ConfirmAsync(Guid appointmentId);
        Task CompleteAsync(Guid appointmentId);
        Task NoShowAsync(Guid appointmentId);
    }
}
