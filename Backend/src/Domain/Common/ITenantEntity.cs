namespace DentalHealthSaaS.Backend.src.Domain.Common
{
    /// <summary>
    /// Represents an entity that is associated with a tenant in a multi-tenant application.
    /// </summary>
    /// <remarks>Implement this interface to identify entities that belong to a specific tenant. The <see
    /// cref="TenantId"/> property should be set to the unique identifier of the tenant that owns the entity.</remarks>
    public interface ITenantEntity
    {
        Guid TenantId { get; set; }
    }
}
