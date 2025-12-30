using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using DentalHealthSaaS.Backend.src.Infrastructure.Identity;
using DentalHealthSaaS.Backend.src.Infrastructure.Tenant;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddAuthorization();
            builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DentalHealthSaaS"));
                options.AddInterceptors(
                    sp.GetRequiredService<AuditSaveChangesInterceptor>());
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<ITenantContext, TenantContext>();

            builder.Services.AddScoped<AuditSaveChangesInterceptor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.Run();
        }
    }
}
