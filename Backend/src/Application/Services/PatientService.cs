using DentalHealthSaaS.Backend.src.Application.Abstractions.Patients;
using DentalHealthSaaS.Backend.src.Application.DTOs.Patients;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _db;

        public PatientService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            var patient = dto.ToEntity();

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            return patient.ToDto();
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.Id == id) ?? throw new Exception("Patient Not Found.");
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<PatientDto>> GetAllAsync()
        {
            return await _db.Patients
                .AsNoTracking()
                .Select(p => p.ToDto())
                .ToListAsync();
        }

        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var patient = await _db.Patients
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Patient Not Found.");
            return patient.ToDto();
        }

        public async Task UpdateAsync(Guid id, UpdatePatientDto dto)
        {
            var patient = await _db.Patients
                .FirstOrDefaultAsync (x => x.Id == id) 
                ?? throw new Exception("Patient Not Found");

            patient.PatientCode = dto.PatientCode;
            patient.Name = dto.Name;
            patient.Age = dto.Age;
            patient.Gender = dto.Gender;
            patient.BirthDate = dto.BirthDate;
            patient.Phone = dto.Phone;
            patient.Email = dto.Email;
            patient.Address = dto.Address;
            patient.EmergencyContact = dto.EmergencyContact;

            await _db.SaveChangesAsync();
        }
    }
}
