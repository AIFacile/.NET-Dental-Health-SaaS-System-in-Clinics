using DentalHealthSaaS.Backend.src.Application.Abstractions.Tenant;
using DentalHealthSaaS.Backend.src.Domain.Appointments;
using DentalHealthSaaS.Backend.src.Domain.Auditing;
using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Diagnoses;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.OralXrayImages;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Payments;
using DentalHealthSaaS.Backend.src.Domain.Tenants;
using DentalHealthSaaS.Backend.src.Domain.Treatments;
using DentalHealthSaaS.Backend.src.Domain.Users;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantContext _tenantContext;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantContext tenantContext) 
            : base(options)
        {
            _tenantContext = tenantContext;
        }

        public Guid? CurrentTenantId =>
            _tenantContext.HasTenant ? _tenantContext.TenantId : null!;

        // ========== SaaS ==========
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        // ========== Core Medical ==========
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Visit> Visits => Set<Visit>();
        public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
        public DbSet<DiagnosisItem> DiagnosisItems => Set<DiagnosisItem>();
        public DbSet<TreatmentPlan> TreatmentPlans => Set<TreatmentPlan>();
        public DbSet<TreatmentStep> TreatmentSteps => Set<TreatmentStep>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
        public DbSet<OralXrayImage> OralXrayImages => Set<OralXrayImage>();

        // ========== Business ==========
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Payment> Payments => Set<Payment>();

        // ========== Auditing ==========
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Tenant filter
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(CreateTenantFilter(entityType.ClrType));
                }

                // Soft delete filter
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }

            ConfigureBaseEntity(modelBuilder);
            ConfigureTenant(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureVisit(modelBuilder);
            ConfigureDiagnosis(modelBuilder);
            ConfigureTreatment(modelBuilder);
            ConfigureAppointment(modelBuilder);
            ConfigureHealthRecord(modelBuilder);
            ConfigureOralXrayImage(modelBuilder);
            ConfigurePayment(modelBuilder);
            ConfigureOthers(modelBuilder);

            // SeedData(modelBuilder);

        }

        private LambdaExpression CreateTenantFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");

            // e.TenantId
            var entityTenantId = Expression.Convert(
                Expression.Property(parameter, 
                nameof(ITenantEntity.TenantId)),
                typeof(Guid?));


            // this.CurrentTenantId
            var contextTenantId = Expression.Property(
                Expression.Constant(this),
                nameof(CurrentTenantId));

            // CurrentTenantId == null
            var noTenant = Expression.Equal(
                contextTenantId,
                Expression.Constant(null, typeof(Guid?)));

            // e.TenantId == CurrentTenantId
            var tenantMatch = Expression.Equal(
                entityTenantId,
                contextTenantId);

            // CurrentTenantId == null || e.TenantId == CurrentTenantId
            var body = Expression.OrElse(noTenant, tenantMatch);

            return Expression.Lambda(body, parameter);
        }

        private LambdaExpression CreateSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");

            var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var body = Expression.Equal(property, Expression.Constant(false));

            return Expression.Lambda(body, parameter);
        }


        // ----------------------------------------------------
        // BaseEntity
        // ----------------------------------------------------
        private static void ConfigureBaseEntity(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<Guid>("TenantId")
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex("TenantId");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property<bool>("IsDeleted")
                        .HasDefaultValue(false);
                }
            }
        }

        // ----------------------------------------------------
        // Tenant / User / Role
        // ----------------------------------------------------
        private static void ConfigureTenant(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Code).IsUnique();

                entity.Property(x => x.Name)
                      .HasMaxLength(200)
                      .IsRequired();
            });
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Username).IsUnique();

                entity.Property(x => x.Username)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.RealName)
                      .HasMaxLength(200)
                      .IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.RoleId });

                entity.HasOne(x => x.User)
                      .WithMany(x => x.UserRoles)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Role)
                      .WithMany()
                      .HasForeignKey(x => x.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // ----------------------------------------------------
        // Patient / Visit
        // ----------------------------------------------------
        private static void ConfigurePatient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PatientCode)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.HasMany(x => x.Visits)
                      .WithOne(x => x.Patient)
                      .HasForeignKey(x => x.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureVisit(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Patient)
                      .WithMany(x => x.Visits)
                      .HasForeignKey(x => x.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Doctor)
                      .WithMany()
                      .HasForeignKey(x => x.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Appointment)
                      .WithOne()
                      .HasForeignKey<Visit>(v => v.AppointmentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }

        // ----------------------------------------------------
        // Diagnosis
        // ----------------------------------------------------
        private static void ConfigureDiagnosis(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Visit)
                      .WithMany(x => x.Diagnoses)
                      .HasForeignKey(x => x.VisitId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DiagnosisItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Diagnosis)
                      .WithMany(x => x.Items)
                      .HasForeignKey(x => x.DiagnosisId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // ----------------------------------------------------
        // Treatment
        // ----------------------------------------------------
        private static void ConfigureTreatment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreatmentPlan>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EstimatedCost)
                      .HasPrecision(18, 2);

                entity.HasOne(x => x.Visit)
                      .WithMany(x => x.TreatmentPlans)
                      .HasForeignKey(x => x.VisitId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TreatmentStep>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Cost)
                      .HasPrecision(18, 2);

                entity.HasOne(x => x.TreatmentPlan)
                      .WithMany(x => x.Steps)
                      .HasForeignKey(x => x.TreatmentPlanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // ----------------------------------------------------
        // Appointment
        // ----------------------------------------------------
        public static void ConfigureAppointment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // Appointment → Patient
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment → Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // ----------------------------------------------------
        // HealthRecord
        // ----------------------------------------------------
        public static void ConfigureHealthRecord(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // HealthRecord → Patient
            modelBuilder.Entity<HealthRecord>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthRecord → Visit
            modelBuilder.Entity<HealthRecord>()
                .HasOne(a => a.Visit)
                .WithMany(v => v.HealthRecords)
                .HasForeignKey(a => a.VisitId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // ----------------------------------------------------
        // OralXrayImage
        // ----------------------------------------------------
        public static void ConfigureOralXrayImage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OralXrayImage>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // HealthRecord → Patient
            modelBuilder.Entity<OralXrayImage>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthRecord → Visit
            modelBuilder.Entity<OralXrayImage>()
                .HasOne(a => a.Visit)
                .WithMany(v => v.OralXrayImages)
                .HasForeignKey(a => a.VisitId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // ----------------------------------------------------
        // Payment
        // ----------------------------------------------------
        public static void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // HealthRecord → Patient
            modelBuilder.Entity<Payment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthRecord → Visit
            modelBuilder.Entity<Payment>()
                .HasOne(a => a.Visit)
                .WithMany(v => v.Payments)
                .HasForeignKey(a => a.VisitId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // ----------------------------------------------------
        // Other entities
        // ----------------------------------------------------
        private static void ConfigureOthers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
        }

        // ----------------------------------------------------
        // Data Seeding
        // ----------------------------------------------------
        private static void SeedData(ModelBuilder modelBuilder)
        {
            // =========================
            // Tenant
            // =========================
            modelBuilder.Entity<Tenant>().HasData(new Tenant
            {
                Id = SeedConstants.Tenant1Id,
                Code = "CLINIC001",
                Name = "Sunshine Dental Clinic",
                Address = "123 Main Street",
                Phone = "123-456-7890",
                Email = "contact@sunshine-dental.com",
                SubscriptionPlan = "Professional",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1),
                CreatedBy = SeedConstants.AdminUserId,
                UpdatedAt = new DateTime(2024, 1, 1),
                UpdatedBy = SeedConstants.AdminUserId,
                TenantId = SeedConstants.Tenant1Id
            });

            // =========================
            // Roles
            // =========================
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = SeedConstants.RoleAdminId, Name = RoleName.SuperAdmin },
                new Role { Id = SeedConstants.RoleDoctorId, Name = RoleName.Doctor }
            );

            // =========================
            // Users
            // =========================
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = SeedConstants.AdminUserId,
                    Username = "SuperAdmin",
                    RealName = "You Know Who",
                    Email = "admin@clinic.com",
                    // Password 123456
                    PasswordHash = Convert.FromHexString(
                        "1DC2AB34B210E11012B47155A2BDD42DEA" +
                        "F5ED1EA44FF874F77BD2E4E3E63D4C5E8EA44" +
                        "B9C83FD6C8C16A8E0E3F635DEC5403C7A7F09" +
                        "F035BEE4FBA8373C95EA"),
                    PasswordSalt = Convert.FromHexString(
                        "976D3F96656EB44DFC42C03B0521ADC2AD4" +
                        "D3F3D2301D2068333E41D2D88563296C15E5E" +
                        "96DA88BC37A0ED95C990628837C5825DB10BF" +
                        "73CE19CDFF1121267F38D4530E8CB2C94C5ED" +
                        "53DF5A7220628D3C69EA771AFCBB241AFEFE6" +
                        "F967EB5FE6672A7EA4AF46C726E9291E3721E" +
                        "F46C61A4F7871870942CC5B6E7B658051836"),
                    IsActive = true,
                    TenantId = SeedConstants.Tenant1Id,
                    CreatedAt = new DateTime(2024, 1, 1),
                    CreatedBy = SeedConstants.AdminUserId,
                    UpdatedAt = new DateTime(2024, 1, 1),
                    UpdatedBy = SeedConstants.AdminUserId
                },
                new User
                {
                    Id = SeedConstants.DoctorUserId,
                    Username = "dr.smith",
                    RealName = "Dr. John Smith",
                    Email = "dr.smith@clinic.com",
                    // Password 654321
                    PasswordHash = Convert.FromHexString(
                        "D7EE820CC731C4B520A418BAF2D1420E7E32" +
                        "F2E0F824647C492768802E569011D6AFDDD05D" +
                        "BE025E7394AD9293A6807145DB51C64020C8E8" +
                        "C5D9463A98D7121F"),
                    PasswordSalt = Convert.FromHexString(
                        "F342F5720D538CA0BBB83326AA5DA1CDB1155" +
                        "EB1BF629318CE2FD6340B5D1E9DAF8C64D9D354" +
                        "8A4E8674BADB7BA26F7FCF050B5DE71CD4A63EF" +
                        "8B6246E0010835CC59B73E09D52327F2C9DBEB7" +
                        "99AB3F9E8AADEF202C71425E0583AA7C738A65C" +
                        "6CDE7F86DD54F81A96782AA1FC6743F33295D64" +
                        "571272B07D65371A3543BE2E"),
                    IsActive = true,
                    TenantId = SeedConstants.Tenant1Id,
                    CreatedAt = new DateTime(2024, 1, 1),
                    CreatedBy = SeedConstants.AdminUserId,
                    UpdatedAt = new DateTime(2024, 1, 1),
                    UpdatedBy = SeedConstants.AdminUserId
                }
            );

            // =========================
            // UserRoles
            // =========================
            modelBuilder.Entity<UserRole>().HasData(
                new { UserId = SeedConstants.AdminUserId, RoleId = SeedConstants.RoleAdminId },
                new { UserId = SeedConstants.DoctorUserId, RoleId = SeedConstants.RoleDoctorId }
            );

            // =========================
            // Patient
            // =========================
            modelBuilder.Entity<Patient>().HasData(new Patient
            {
                Id = SeedConstants.Patient1Id,
                TenantId = SeedConstants.Tenant1Id,
                PatientCode = "PAT-001",
                Name = "Alice Brown",
                Gender = "Female",
                Age = 30,
                BirthDate = new DateTime(1995, 5, 20),
                Phone = "555-1234",
                Address = "456 Oak Street",
                CreatedAt = new DateTime(2024, 1, 10),
                CreatedBy = SeedConstants.DoctorUserId,
                UpdatedAt = new DateTime(2024, 1, 10),
                UpdatedBy = SeedConstants.DoctorUserId,
                IsDeleted = false
            });

            // =========================
            // Visit
            // =========================
            modelBuilder.Entity<Visit>().HasData(new Visit
            {
                Id = SeedConstants.Visit1Id,
                TenantId = SeedConstants.Tenant1Id,
                PatientId = SeedConstants.Patient1Id,
                VisitDate = new DateTime(2024, 1, 15),
                VisitType = "Initial",
                Status = VisitStatus.Open,
                DoctorId = SeedConstants.DoctorUserId,
                Notes = "Initial consultation",
                CreatedAt = new DateTime(2024, 1, 15),
                CreatedBy = SeedConstants.DoctorUserId,
                UpdatedAt = new DateTime(2024, 1, 15),
                UpdatedBy = SeedConstants.DoctorUserId
            });

            // =========================
            // Diagnosis
            // =========================
            modelBuilder.Entity<Diagnosis>().HasData(new Diagnosis
            {
                Id = SeedConstants.Diagnosis1Id,
                TenantId = SeedConstants.Tenant1Id,
                VisitId = SeedConstants.Visit1Id,
                PatientId = SeedConstants.Patient1Id,
                DiagnosisDate = new DateTime(2024, 1, 15),
                DoctorId = SeedConstants.DoctorUserId,
                Status = DiagnosisStatus.Confirmed,
                Summary = "Dental caries detected",
                CreatedAt = new DateTime(2024, 1, 15),
                CreatedBy = SeedConstants.DoctorUserId,
                UpdatedAt = new DateTime(2024, 1, 15),
                UpdatedBy = SeedConstants.DoctorUserId
            });

            modelBuilder.Entity<DiagnosisItem>().HasData(new DiagnosisItem
            {
                Id = SeedConstants.DiagnosisItem1Id,
                DiagnosisId = SeedConstants.Diagnosis1Id,
                ToothPosition = "Molar 36",
                DiseaseName = "Caries",
                Severity = "Moderate"
            });

            // =========================
            // Treatment
            // =========================
            modelBuilder.Entity<TreatmentPlan>().HasData(new TreatmentPlan
            {
                Id = SeedConstants.TreatmentPlan1Id,
                TenantId = SeedConstants.Tenant1Id,
                VisitId = SeedConstants.Visit1Id,
                PatientId = SeedConstants.Patient1Id,
                PlanType = "Filling",
                EstimatedCost = 200m,
                Status = TreatmentPlanStatus.Approved,
                CreatedAt = new DateTime(2024, 1, 16),
                CreatedBy = SeedConstants.DoctorUserId,
                UpdatedAt = new DateTime(2024, 1, 16),
                UpdatedBy = SeedConstants.DoctorUserId
            });

            modelBuilder.Entity<TreatmentStep>().HasData(new TreatmentStep
            {
                Id = SeedConstants.TreatmentStep1Id,
                TreatmentPlanId = SeedConstants.TreatmentPlan1Id,
                StepOrder = 1,
                Description = "Composite filling",
                Cost = 200m,
                Status = TreatmentStepStatus.Pending
            });
        }
    }
}
