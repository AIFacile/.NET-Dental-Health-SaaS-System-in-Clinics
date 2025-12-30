namespace DentalHealthSaaS.Backend.src.Infrastructure.Tenant
{
    public interface ITenantContext
    {
        Guid TenantId { get; }
    }

    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _http;

        public TenantContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public Guid TenantId =>
            Guid.Parse(_http.HttpContext!
                .User.FindFirst("tenant_id")!.Value);
    }
}
