using DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Diagnoses;
using DentalHealthSaaS.Backend.src.Application.Abstractions.HealthRecords;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Patients;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Payments;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;
using DentalHealthSaaS.Backend.src.Application.Abstractions.TreatmentPlans;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Visits;
using DentalHealthSaaS.Backend.src.Application.Services;
using DentalHealthSaaS.Backend.src.Infrastructure.Auth;
using DentalHealthSaaS.Backend.src.Infrastructure.Auth.Handlers;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using DentalHealthSaaS.Backend.src.Infrastructure.Identity;
using DentalHealthSaaS.Backend.src.Infrastructure.Tenants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace DentalHealthSaaS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddCors();

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<AuditSaveChangesInterceptor>();

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

            // Scope buisness services
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IDiagnosisService, DiagnosisService>();
            builder.Services.AddScoped<IVisitService, VisitService>();
            builder.Services.AddScoped<ITreatmentPlanService, TreatmentPlanService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            // Scope rescource-based authorization handler
            builder.Services.AddScoped<IAuthorizationHandler, OwnsVisitHandler>();

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
            builder.Services.AddAppAuthorization();

            // Add AuditLogs saving service.
            builder.Services.AddScoped<AuditSaveChangesInterceptor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader()
            .WithOrigins("http://localhost:4200", "https://localhost:4200"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
