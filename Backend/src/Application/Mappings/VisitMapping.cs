using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class VisitMapping
    {
        public static VisitDto ToDto(this Visit visit)
        {
            return new VisitDto
            {
                Id = visit.Id,
                PatientId = visit.PatientId,
                DoctorId = visit.DoctorId,
                Status = visit.Status,
                VisitDate = visit.VisitDate,
                VisitType = visit.VisitType,
                Notes = visit.Notes,
            };
        }

        public static Visit ToEntity(this CreateVisitDto dto)
        {
            return new Visit
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                Status = VisitStatus.Open,
                VisitDate = dto.VisitDate,
                VisitType = dto.VisitType,
                Notes = dto.Notes,
            };
        }
    }
}
