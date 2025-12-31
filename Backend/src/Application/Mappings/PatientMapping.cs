using DentalHealthSaaS.Backend.src.Application.DTOs.Patients;
using DentalHealthSaaS.Backend.src.Domain.Patients;

namespace DentalHealthSaaS.Backend.src.Application.Mappings
{
    public static class PatientMapping
    {
        public static PatientDto ToDto(this Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                PatientCode = patient.PatientCode,
                Name = patient.Name,
                Gender = patient.Gender,
                Age = patient.Age,
                BirthDate = patient.BirthDate,
                Phone = patient.Phone,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContact = patient.EmergencyContact,
            };
        }

        public static Patient ToEntity(this CreatePatientDto dto)
        {
            return new Patient
            {
                Id = Guid.NewGuid(),
                PatientCode = dto.PatientCode,
                Name = dto.Name,
                Gender = dto.Gender,
                Age = dto.Age,
                BirthDate = dto.BirthDate,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                EmergencyContact = dto.EmergencyContact,
            };
        }
    }
}
