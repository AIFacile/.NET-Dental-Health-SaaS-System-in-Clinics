using DentalHealthSaaS.Backend.src.Application.Abstractions.HealthRecords;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.DTOs.HealthRecords;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class HealthRecordService (ApplicationDbContext db, IUserContext user) : IHealthRecordService
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IUserContext _user = user;

        public async Task<HealthRecordDto> CreateAsync(Guid patientId, CreateHealthRecordDto dto)
        {
            var visit = await _db.Visits
                .FirstOrDefaultAsync(v => v.Id == dto.VisitId &&
                v.PatientId == patientId)
                ?? throw new Exception("Visit does not belong to patient.");

            if (visit.Status != VisitStatus.Open &&
                visit.Status != VisitStatus.InTreatment)
                throw new Exception("Cannot add health record in current visit status.");

            var record = new HealthRecord
            {
                Id = Guid.NewGuid(),
                VisitId = visit.Id,
                PatientId = patientId,
                ToothPosition = dto.ToothPosition,
                DentalStatus = dto.DentalStatus,
                Notes = dto.Notes,
                RecordedAt = DateTime.UtcNow,
            };

            _db.HealthRecords.Add(record);
            await _db.SaveChangesAsync();

            return record.ToDto();

        }

        public async Task<IReadOnlyList<HealthRecordDto>> GetByPatietnAsync(Guid patientId)
        {
            return await _db.HealthRecords
                .Where(r => r.PatientId == patientId)
                .AsNoTracking()
                .Include(p => p.Patient)
                .Include(v => v.Visit)
                .OrderByDescending(r => r.RecordedAt)
                .Select(r => r.ToDto())
                .ToListAsync();
        }

        public async Task<IReadOnlyList<HealthRecordDto>> GetByVisitAsync(Guid visitId)
        {
            return await _db.HealthRecords
                .Where(r => r.VisitId == visitId)
                .AsNoTracking()
                .Include(p => p.Patient)
                .Include(v => v.Visit)
                .OrderBy(r => r.ToothPosition)
                .Select(r => r.ToDto())
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateHealthRecordDto dto)
        {
            var record = await _db.HealthRecords
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new Exception("Health record not found.");

            record.DentalStatus = dto.DentalSatus;
            record.Notes = dto.Notes;

            await _db.SaveChangesAsync();
        }
    }
}
