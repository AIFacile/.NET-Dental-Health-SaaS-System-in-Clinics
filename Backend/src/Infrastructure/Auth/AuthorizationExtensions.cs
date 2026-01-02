using DentalHealthSaaS.Backend.src.Application.Security;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Auth
{
    public static class AuthorizationExtensions 
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var field in typeof(Permissions).GetFields())
                {
                    var permission = field.GetValue(null)?.ToString();
                    if (permission == null) continue;

                    options.AddPolicy(permission, policy =>
                        policy.RequireClaim("permissions", permission));
                }
            });

            return services;
        }
    }
}
