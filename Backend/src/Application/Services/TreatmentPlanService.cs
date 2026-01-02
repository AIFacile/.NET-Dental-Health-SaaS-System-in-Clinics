using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Abstractions.TreatmentPlans;
using DentalHealthSaaS.Backend.src.Application.DTOs.TreatmentPlans;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.Treatments;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class TreatmentPlanService (ApplicationDbContext db, IUserContext user) : ITreatmentPlanService
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IUserContext _user = user;

        public async Task<TreatmentPlanDto> CreateAsync(Guid patientId, CreateTreatmentPlanDto dto)
        {
            var visit = await _db.Visits
                .FirstOrDefaultAsync(v => v.Id == dto.VisitId && v.PatientId == patientId)
                ?? throw new Exception("Visit does not belong to patient.");

            if (visit.Status != VisitStatus.Open &&
                visit.Status != VisitStatus.InTreatment)
                throw new Exception("Cannot create treatment plan in current visit status.");

            var plan = new TreatmentPlan
            {
                Id = Guid.NewGuid(),
                VisitId = visit.Id,
                PatientId = patientId,
                PlanType = dto.PlanType,
                Status = TreatmentPlanStatus.Draft,
            };

            foreach (var step in dto.Steps)
            {
                plan.Steps.Add(new TreatmentStep
                {
                    Id = Guid.NewGuid(),
                    StepOrder = step.StepOrder,
                    Description = step.Description,
                    Cost = step.Cost,
                    Status = TreatmentStepStatus.Pending
                });
            }

            plan.EstimatedCost = plan.Steps.Sum(s => s.Cost);

            _db.TreatmentPlans.Add(plan);
            await _db.SaveChangesAsync();

            return plan.ToDto();
        }

        public async Task<TreatmentPlanDto> GetByIdAsync(Guid id)
        {
            var plan = await _db.TreatmentPlans
                .Include(p => p.Steps)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Treatment plan not found.");

            return plan.ToDto();
        }

        public async Task<IReadOnlyList<TreatmentPlanDto>> GetByVisitAsync(Guid visitId)
        {
            return await _db.TreatmentPlans
                .Where(p => p.VisitId == visitId)
                .Include(p => p.Steps)
                .AsNoTracking()
                .Select(p => p.ToDto())
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateTreatmentPlanDto dto)
        {
            var plan = await _db.TreatmentPlans
                .Include(p => p.Steps)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Treatment plan not found.");

            plan.Status = dto.Status;

            var existingSteps = plan.Steps.ToDictionary(s => s.Id);

            foreach (var stepDto in dto.Steps)
            {
                if (stepDto.Id == null)
                {
                    plan.Steps.Add(new TreatmentStep
                    {
                        Id = Guid.NewGuid(),
                        TreatmentPlanId = plan.Id,
                        StepOrder = stepDto.StepOrder,
                        Description = stepDto.Description,
                        Cost = stepDto.Cost,
                        Status = stepDto.Status
                    });
                    continue;
                }

                if (!existingSteps.TryGetValue(stepDto.Id.Value, out var existing))
                    throw new Exception($"TreatmentStep {stepDto.Id} not found.");

                existing.StepOrder = stepDto.StepOrder;
                existing.Description = stepDto.Description;
                existing.Cost = stepDto.Cost;
                existing.Status = stepDto.Status;
            }

            var dtoStepIds = dto.Steps
                .Where(s => s.Id.HasValue)
                .Select(s => s.Id!.Value)
                .ToHashSet();

            var toRemove = plan.Steps
                .Where(s => !dtoStepIds.Contains(s.Id))
                .ToList();

            foreach (var step in toRemove)
            {
                _db.TreatmentSteps.Remove(step);
            }

            plan.EstimatedCost = plan.Steps.Sum(s => s.Cost);

            await _db.SaveChangesAsync();
        }
    }
}
