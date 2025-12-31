using DentalHealthSaaS.Backend.src.Application.Abstractions.Visits;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class VisitService(ApplicationDbContext db) : IVisitService
    {
        private readonly ApplicationDbContext _db = db;

        async Task IVisitService.CancelAsync(Guid visitId)
        {
            var visit = await GetVisitOrThrow(visitId);

            if (visit.Status == "Completed") throw new Exception("Completed visit cannot be canceled.");

            visit.Status = "Cancelled";
            await _db.SaveChangesAsync();
        }

        async Task IVisitService.CompleteAsync(Guid visitId)
        {
            var visit = await GetVisitOrThrow(visitId);

            if (visit.Status != "In Treatment")
                throw new Exception("Treatment not completed.");

            visit.Status = "Completed";
            await _db.SaveChangesAsync();
        }

        public async Task<Guid> CreateAsync(CreateVisitDto dto)
        {
            var patientExists = await _db.Patients
                .AnyAsync(p => p.Id == dto.PatientId);

            if (!patientExists)
                throw new Exception("Patient does not exist.");

            var visit = new Visit
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                VisitDate = dto.VisitDate,
                Status = "Open"
            };

            _db.Visits.Add(visit);
            await _db.SaveChangesAsync();

            return visit.Id;
        }

        public async Task<VisitDto> GetById(Guid id)
        {
            var visit = await _db.Visits.FirstOrDefaultAsync(v => v.Id == id);

            return visit == null 
                ? throw new Exception("Visit doesn't exist in database.") 
                : visit.ToDto();
        }

        public async Task<IReadOnlyList<VisitDto>> GetByPatientsAsync(Guid patientId)
        {
            var patientExists = await _db.Patients
                .AnyAsync(p => p.Id == patientId);
            if (!patientExists) throw new Exception("Patient not found.");

            return await _db.Visits
                .Where(v => v.PatientId == patientId)
                .AsNoTracking()
                .Select(v => v.ToDto())
                .ToListAsync();
        }

        public async Task StartDiagnosisAsync(Guid visitId)
        {
            var visit = await GetVisitOrThrow(visitId);

            if (visit.Status != "Open")
                throw new Exception("Visit is not in open status.");

            visit.Status = "Diagnosed";
            await _db.SaveChangesAsync();
        }

        public async Task StartTreatmentAsync(Guid visitId)
        {
            var visit = await GetVisitOrThrow(visitId);

            if (visit.Status != "Diagnosed")
                throw new Exception("Diagnosis not completed.");

            visit.Status = "In Treatment";
            await _db.SaveChangesAsync();
        }

        private async Task<Visit> GetVisitOrThrow(Guid id) => await _db.Visits
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new Exception("Visit record does not exist.");

    }
}
