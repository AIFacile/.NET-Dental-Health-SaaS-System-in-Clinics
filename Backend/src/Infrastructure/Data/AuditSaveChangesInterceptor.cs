using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;
using DentalHealthSaaS.Backend.src.Domain.Auditing;
using DentalHealthSaaS.Backend.src.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

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
    public sealed class AuditSaveChangesInterceptor(
        IUserContext userContext,
        ITenantContext tenantContext) : SaveChangesInterceptor
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ITenantContext _tenantContext = tenantContext;

        // -----------------------------
        // Sync
        // -----------------------------
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ProcessAudit(eventData.Context!);
            return base.SavingChanges(eventData, result);
        }

        // -----------------------------
        // Async
        // -----------------------------
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ProcessAudit(eventData.Context!);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // =====================================================
        // Core Logic
        // =====================================================
        private void ProcessAudit(DbContext context)
        {
            var now = DateTime.UtcNow;

            if (!_tenantContext.HasTenant || !_userContext.HasUser)
                return;

            var tenantId = _tenantContext.TenantId;
            var userId = _userContext.UserId;

            var auditLogs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog)
                    continue;

                if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                    continue;

                var entityId = GetPrimaryKey(entry);
                if (entityId == null)
                    continue;

                if (entry.State == EntityState.Added &&
                    entry.Metadata.FindProperty("UpdatedBy") != null &&
                    entry.Metadata.FindProperty("UpdatedAt") != null &&
                    entry.Metadata.FindProperty("CreatedBy") != null &&
                    entry.Metadata.FindProperty("CreatedAt") != null &&
                    entry.Metadata.FindProperty("TenantId") != null)
                {
                    entry.Property("TenantId").CurrentValue = tenantId;
                    entry.Property("UpdatedBy").CurrentValue = userId;
                    entry.Property("UpdatedAt").CurrentValue = now;
                    entry.Property("CreatedBy").CurrentValue = userId;
                    entry.Property("CreatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Modified &&
                    entry.Metadata.FindProperty("UpdatedBy") != null &&
                    entry.Metadata.FindProperty("UpdatedAt") != null &&
                    entry.Metadata.FindProperty("TenantId") != null)
                {
                    entry.Property("TenantId").CurrentValue = tenantId;
                    entry.Property("UpdatedBy").CurrentValue = userId;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Deleted &&
                    entry.Metadata.FindProperty("UpdatedBy") != null &&
                    entry.Metadata.FindProperty("UpdatedAt") != null &&
                    entry.Metadata.FindProperty("IsDeleted") != null &&
                    entry.Metadata.FindProperty("TenantId") != null)
                {
                    entry.Property("TenantId").CurrentValue = tenantId;
                    entry.Property("UpdatedBy").CurrentValue = userId;
                    entry.Property("UpdatedAt").CurrentValue = now;
                    entry.Property("IsDeleted").CurrentValue = true;
                }

                auditLogs.Add(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    UserId = userId,
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entityId,
                    Action = entry.State.ToString(),
                    Timestamp = now,
                    DataSnapshot = SerializeEntity(entry)
                });
            }

            if (auditLogs.Count > 0)
                context.Set<AuditLog>().AddRange(auditLogs);
        }

        // =====================================================
        // Helpers
        // =====================================================
        private static string? GetPrimaryKey(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null)
                return null;

            var values = key.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString());

            return string.Join(",", values);
        }

        private static string SerializeEntity(EntityEntry entry)
        {
            var data = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey())
                    continue;

                data[prop.Metadata.Name] =
                    entry.State == EntityState.Deleted
                        ? prop.OriginalValue
                        : prop.CurrentValue;
            }

            return JsonSerializer.Serialize(data);
        }
    }
}
