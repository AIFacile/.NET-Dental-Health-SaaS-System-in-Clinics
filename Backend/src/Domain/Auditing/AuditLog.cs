namespace DentalHealthSaaS.Backend.src.Domain.Auditing
{
    /// <summary>
    /// Represents an audit log entry that records actions performed on entities within a tenant context.
    /// </summary>
    /// <remarks>Use this class to track changes or operations performed by users on specific entities for
    /// auditing and compliance purposes. Each entry includes information about the affected entity, the action taken,
    /// the user who performed the action, and the time the action occurred.</remarks>
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }

        public required string EntityName { get; set; }
        public required string EntityId { get; set; }

        public required string Action { get; set; }
        public DateTime Timestamp { get; set; }

        public string? DataSnapshot { get; set; }
    }
}
