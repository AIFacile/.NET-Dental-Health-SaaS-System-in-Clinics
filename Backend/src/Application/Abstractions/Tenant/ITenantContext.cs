namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant
{
    public interface ITenantContext
    {
        Guid TenantId { get; }
        bool HasTenant { get; }
    }
}
