using DentalHealthSaaS.Backend.src.Application.Abstractions.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.DTOs.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.Diagnoses;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class DiagnosisService(
        ApplicationDbContext db,
        IUserContext user) : IDiagnosisService
    {
        // DI
        private readonly ApplicationDbContext _db = db;
        private readonly IUserContext _user = user;

        // Service Core
        public async Task<DiagnosisDto> CreateAsync(Guid patientId, CreateDiagnosisDto dto)
        {
            var patient = await _db.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId) 
                ?? throw new Exception("Patient not found.");

            var visit = await _db.Visits
                .FirstOrDefaultAsync(v => v.Id == dto.VisitId 
                && v.PatientId == patientId) 
                ?? throw new Exception("Visit does not belong to a patient.");

            if (visit.Status != VisitStatus.Diagnosed && visit.Status != VisitStatus.InTreatment)
                throw new Exception("Cannot add diagnosis to visit in current status.");                

            var diagnosis = dto.ToEntity(patientId, _user.UserId);

            _db.Diagnoses.Add(diagnosis);
            await _db.SaveChangesAsync();

            return diagnosis.ToDto();
        }

        public async Task<DiagnosisDto> GetByIdAsync(Guid id)
        {
            var diagnosis = await _db.Diagnoses
                .Include(d => d.Items)
                .FirstOrDefaultAsync(d => d.Id == id) 
                ?? throw new Exception("Diagnosis not found.");

            return diagnosis.ToDto();

        }

        public async Task<IReadOnlyList<DiagnosisDto>> GetByPatientAsync(Guid patientId)
        {
            var patientExists = await _db.Patients
                .AnyAsync(p => p.Id == patientId);
            if (!patientExists) throw new Exception("Patient not found");

            return await _db.Diagnoses
                .Where(d => d.PatientId == patientId)
                .Include(d => d.Items)
                .AsNoTracking()
                .Select(d => d.ToDto())
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateDiagnosisDto dto)
        {
            var diagnosis = await _db.Diagnoses
                .Include(d => d.Items)
                .FirstOrDefaultAsync(d => d.Id == id) 
                ?? throw new Exception("Diagnosis not found.");

            diagnosis.Status = dto.Status;
            diagnosis.Summary = dto.Summary;

            var existingItems = diagnosis.Items.ToDictionary(i => i.Id);

            foreach (var itemDto in dto.Items)
            {
                if (itemDto.Id == null)
                {
                    diagnosis.Items.Add(new DiagnosisItem
                    {
                        Id = Guid.NewGuid(),
                        DiagnosisId = diagnosis.Id,
                        ToothPosition = itemDto.ToothPosition,
                        DiseaseName = itemDto.DiseaseName,
                        Severity = itemDto.Severity,
                        Notes = itemDto.Notes,
                    });
                    continue;
                }

                if (!existingItems.TryGetValue(itemDto.Id.Value, out var existing))
                    throw new Exception($"DiagnosisItem {itemDto.Id} not found.");

                existing.ToothPosition = itemDto.ToothPosition;
                existing.DiseaseName = itemDto.DiseaseName;
                existing.Severity = itemDto.Severity;
                existing.Notes = itemDto.Notes;
            }

            var dtoItemIds = dto.Items
                .Where(i => i.Id.HasValue)
                .Select(i => i.Id!.Value)
                .ToHashSet();

            var itemsToRemove = diagnosis.Items
                .Where(i => !dtoItemIds.Contains(i.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                _db.DiagnosisItems.Remove(item);
            }

            await _db.SaveChangesAsync();
        }
    }
}
