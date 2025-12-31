using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Tenants
{
    /// <summary>
    /// Provides access to the current tenant's unique identifier within the HTTP request context.
    /// </summary>
    /// <remarks>This class retrieves the tenant ID from the authenticated user's claims in the current HTTP
    /// context. It is typically used in multi-tenant applications to identify the tenant associated with the current
    /// request. The tenant ID is expected to be present as a claim named "tenant_id" in the user's identity.</remarks>
    public class TenantContext(IHttpContextAccessor http) : ITenantContext
    {
        private readonly IHttpContextAccessor _http = http;

        public bool HasTenant =>
            _http.HttpContext?
                .User?
                .HasClaim(c => c.Type == "tenant_id") == true;

        public Guid TenantId { 
            get 
            {
                return !HasTenant 
                    ? throw new UnauthorizedAccessException("Tenant not resolved") 
                    : Guid.Parse(_http.HttpContext!.User.FindFirst("tenant_id")!.Value);
            }
        }
    }
}
