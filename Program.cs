using DentalHealthSaaS.Backend.src.Application.Abstractions.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Patients;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Visits;
using DentalHealthSaaS.Backend.src.Application.Services;
using DentalHealthSaaS.Backend.src.Infrastructure.Auth;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using DentalHealthSaaS.Backend.src.Infrastructure.Identity;
using DentalHealthSaaS.Backend.src.Infrastructure.Tenants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DentalHealthSaaS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DentalHealthSaaS"));
                options.AddInterceptors(
                    sp.GetRequiredService<AuditSaveChangesInterceptor>());
            });

            builder.Services.AddHttpContextAccessor();

            // Scope usercontext and tenantcontext to provide user&tenant data to other services.
            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<ITenantContext, TenantContext>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IDiagnosisService, DiagnosisService>();
            builder.Services.AddScoped<IVisitService, VisitService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        //ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            // Add AuditLogs saving service.
            builder.Services.AddScoped<AuditSaveChangesInterceptor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
