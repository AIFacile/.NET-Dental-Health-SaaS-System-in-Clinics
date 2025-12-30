namespace DentalHealthSaaS.Backend.src.Domain.Auditing
{
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
