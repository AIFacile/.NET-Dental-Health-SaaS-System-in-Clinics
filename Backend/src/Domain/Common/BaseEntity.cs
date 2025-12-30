namespace DentalHealthSaaS.Backend.src.Domain.Common
{
    public abstract class BaseEntity : IAuditableEntity, ITenantEntity
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
