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
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

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

            ConfigureBaseEntity(modelBuilder);
            ConfigureTenant(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureVisit(modelBuilder);
            ConfigureDiagnosis(modelBuilder);
            ConfigureTreatment(modelBuilder);
            ConfigureOthers(modelBuilder);

            SeedData(modelBuilder);
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
        // Other entities
        // ----------------------------------------------------
        private static void ConfigureOthers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<OralXrayImage>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Amount)
                      .HasPrecision(18, 2);
            });

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
                new Role { Id = SeedConstants.RoleAdminId, Name = "Admin" },
                new Role { Id = SeedConstants.RoleDoctorId, Name = "Doctor" },
                new Role { Id = SeedConstants.RoleAssistantId, Name = "Assistant" }
            );

            // =========================
            // Users
            // =========================
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = SeedConstants.AdminUserId,
                    Username = "admin",
                    RealName = "System Admin",
                    Email = "admin@clinic.com",
                    PasswordHash = new byte[] { 1, 2, 3 }, // Demo only
                    PasswordSalt = new byte[] { 4, 5, 6 },
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
                    PasswordHash = new byte[] { 7, 8, 9 },
                    PasswordSalt = new byte[] { 10, 11, 12 },
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
                Status = "Open",
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
                Status = "Confirmed",
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
                Status = "Approved",
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
                Status = "Pending"
            });
        }
    }
}
