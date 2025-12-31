using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;

namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Visits
{
    public interface IVisitService
    {
        Task<Guid> CreateAsync(CreateVisitDto dto);
        Task<VisitDto> GetById(Guid id);
        Task<IReadOnlyList<VisitDto>> GetByPatientsAsync(Guid patientId);

        Task StartDiagnosisAsync(Guid visitId);
        Task StartTreatmentAsync(Guid visitId);
        Task CompleteAsync(Guid visitId);
        Task CancelAsync(Guid visitId);
    }
}
