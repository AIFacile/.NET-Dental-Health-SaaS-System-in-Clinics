using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;
using DentalHealthSaaS.Backend.src.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Data
{
    /// <summary>
    /// Intercepts save operations to automatically populate audit and tenant information for entities implementing
    /// audit or tenant interfaces.
    /// </summary>
    /// <remarks>This interceptor sets audit fields such as creation and modification timestamps, user
    /// identifiers, and tenant identifiers on entities that implement the relevant interfaces during save operations.
    /// It is intended to be registered with Entity Framework Core's change tracking pipeline to ensure consistent audit
    /// data across all changes. Thread safety is determined by the underlying contexts provided; ensure that
    /// IUserContext and ITenantContext are scoped appropriately for application's usage.</remarks>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContext _user;
        private readonly ITenantContext _tenant;

        public AuditSaveChangesInterceptor(IUserContext user, ITenantContext tenant) 
        {
            _user = user;
            _tenant = tenant;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context!;
            var now = DateTime.UtcNow;

            Guid? userId = _user.HasUser ? _user.UserId : Guid.Empty;
            Guid? tenantId = _tenant.HasTenant ? _tenant.TenantId : Guid.Empty;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // ===== Audit =====
                if (entry.Entity is IAuditableEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = now;
                        auditable.UpdatedAt = now;

                        if (userId.HasValue)
                        {
                            auditable.CreatedBy = userId.Value;
                            auditable.UpdatedBy = userId.Value;
                        }
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedAt = now;

                        if (userId.HasValue) auditable.UpdatedBy = userId.Value;
                    }
                }

                // ===== Tenant =====
                if (entry.Entity is ITenantEntity tenantEntity && 
                    entry.State == EntityState.Added &&
                    tenantId.HasValue)
                {
                    tenantEntity.TenantId = tenantId.Value;
                }
            }

            foreach (var entry in context.ChangeTracker.Entries())
            {
                // ===== Soft Delete =====
                if (entry.State == EntityState.Deleted &&
                    entry.Entity is ISoftDelete softDelete)
                {
                    entry.State = EntityState.Modified;
                    softDelete.IsDeleted = true;

                    if (entry.Entity is IAuditableEntity auditable)
                    {
                        auditable.UpdatedAt = now;
                        if (userId.HasValue) auditable.UpdatedBy = userId.Value;
                    }
                }
            }


            return base.SavingChanges(eventData, result);
        }
    }
}
