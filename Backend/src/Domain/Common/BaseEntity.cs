namespace DentalHealthSaaS.Backend.src.Domain.Common
{
    /// <summary>
    /// Provides a base class for entities that require auditing and multi-tenancy support.
    /// </summary>
    /// <remarks>This abstract class defines common properties for tracking entity identity, creation and
    /// modification metadata, soft deletion status, and tenant association. It is intended to be inherited by domain
    /// entities that participate in auditing and tenant-based data segregation.</remarks>
    public abstract class BaseEntity : IAuditableEntity, ITenantEntity, ISoftDelete
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
