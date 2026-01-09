using DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class HealthRecordMapping
    {
        public static HealthRecordDto ToDto(this HealthRecord entity)
        {
            return new HealthRecordDto
            {
                Id = entity.Id,
                PatientId = entity.PatientId,
                Patient = entity.Patient,
                VisitId = entity.VisitId,
                Visit = entity.Visit,
                DentalStatus = entity.DentalStatus,
                ToothPosition = entity.ToothPosition,
                RecordedAt = entity.RecordedAt,
                Notes = entity.Notes,
            };
        }
    }
}
