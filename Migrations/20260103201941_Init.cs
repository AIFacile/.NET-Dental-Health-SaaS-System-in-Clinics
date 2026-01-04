using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DentalHealthSaaS.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RealName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visits_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiagnosisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToothPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DentalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthRecords_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OralXrayImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OralXrayImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OralXrayImages_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentPlans_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosisItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiagnosisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToothPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosisItems_Diagnoses_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreatmentPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentSteps_TreatmentPlans_TreatmentPlanId",
                        column: x => x.TreatmentPlanId,
                        principalTable: "TreatmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 2 }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Address", "Code", "CreatedAt", "CreatedBy", "Email", "IsActive", "Name", "Phone", "SubscriptionPlan", "TenantId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "123 Main Street", "CLINIC001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), "contact@sunshine-dental.com", true, "Sunshine Dental Clinic", "123-456-7890", "Professional", new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "Age", "BirthDate", "CreatedAt", "CreatedBy", "Email", "EmergencyContact", "Gender", "Name", "PatientCode", "Phone", "TenantId", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), "456 Oak Street", 30, new DateTime(1995, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), null, null, "Female", "Alice Brown", "PAT-001", "555-1234", new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "IsActive", "PasswordHash", "PasswordSalt", "RealName", "TenantId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), "admin@clinic.com", true, new byte[] { 29, 194, 171, 52, 178, 16, 225, 16, 18, 180, 113, 85, 162, 189, 212, 45, 234, 245, 237, 30, 164, 79, 248, 116, 247, 123, 210, 228, 227, 230, 61, 76, 94, 142, 164, 75, 156, 131, 253, 108, 140, 22, 168, 224, 227, 246, 53, 222, 197, 64, 60, 122, 127, 9, 240, 53, 190, 228, 251, 168, 55, 60, 149, 234 }, new byte[] { 151, 109, 63, 150, 101, 110, 180, 77, 252, 66, 192, 59, 5, 33, 173, 194, 173, 77, 63, 61, 35, 1, 210, 6, 131, 51, 228, 29, 45, 136, 86, 50, 150, 193, 94, 94, 150, 218, 136, 188, 55, 160, 237, 149, 201, 144, 98, 136, 55, 197, 130, 93, 177, 11, 247, 60, 225, 156, 223, 241, 18, 18, 103, 243, 141, 69, 48, 232, 203, 44, 148, 197, 237, 83, 223, 90, 114, 32, 98, 141, 60, 105, 234, 119, 26, 252, 187, 36, 26, 254, 254, 111, 150, 126, 181, 254, 102, 114, 167, 234, 74, 244, 108, 114, 110, 146, 145, 227, 114, 30, 244, 108, 97, 164, 247, 135, 24, 112, 148, 44, 197, 182, 231, 182, 88, 5, 24, 54 }, "You Know Who", new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), "SuperAdmin" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), "dr.smith@clinic.com", true, new byte[] { 215, 238, 130, 12, 199, 49, 196, 181, 32, 164, 24, 186, 242, 209, 66, 14, 126, 50, 242, 224, 248, 36, 100, 124, 73, 39, 104, 128, 46, 86, 144, 17, 214, 175, 221, 208, 93, 190, 2, 94, 115, 148, 173, 146, 147, 166, 128, 113, 69, 219, 81, 198, 64, 32, 200, 232, 197, 217, 70, 58, 152, 215, 18, 31 }, new byte[] { 243, 66, 245, 114, 13, 83, 140, 160, 187, 184, 51, 38, 170, 93, 161, 205, 177, 21, 94, 177, 191, 98, 147, 24, 206, 47, 214, 52, 11, 93, 30, 157, 175, 140, 100, 217, 211, 84, 138, 78, 134, 116, 186, 219, 123, 162, 111, 127, 207, 5, 11, 93, 231, 28, 212, 166, 62, 248, 182, 36, 110, 0, 16, 131, 92, 197, 155, 115, 224, 157, 82, 50, 127, 44, 157, 190, 183, 153, 171, 63, 158, 138, 173, 239, 32, 44, 113, 66, 94, 5, 131, 170, 124, 115, 138, 101, 198, 205, 231, 248, 109, 213, 79, 129, 169, 103, 130, 170, 31, 198, 116, 63, 51, 41, 93, 100, 87, 18, 114, 176, 125, 101, 55, 26, 53, 67, 190, 46 }, "Dr. John Smith", new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), "dr.smith" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("66666666-6666-6666-6666-666666666666") }
                });

            migrationBuilder.InsertData(
                table: "Visits",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DoctorId", "Notes", "PatientId", "Status", "TenantId", "UpdatedAt", "UpdatedBy", "VisitDate", "VisitType" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("66666666-6666-6666-6666-666666666666"), "Initial consultation", new Guid("77777777-7777-7777-7777-777777777777"), 0, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Initial" });

            migrationBuilder.InsertData(
                table: "Diagnoses",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DiagnosisDate", "DoctorId", "PatientId", "Status", "Summary", "TenantId", "UpdatedAt", "UpdatedBy", "VisitId" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("77777777-7777-7777-7777-777777777777"), 2, "Dental caries detected", new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("88888888-8888-8888-8888-888888888888") });

            migrationBuilder.InsertData(
                table: "TreatmentPlans",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EstimatedCost", "PatientId", "PlanType", "Status", "TenantId", "UpdatedAt", "UpdatedBy", "VisitId" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), 200m, new Guid("77777777-7777-7777-7777-777777777777"), "Filling", 2, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("88888888-8888-8888-8888-888888888888") });

            migrationBuilder.InsertData(
                table: "DiagnosisItems",
                columns: new[] { "Id", "DiagnosisId", "DiseaseName", "Notes", "Severity", "ToothPosition" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("99999999-9999-9999-9999-999999999999"), "Caries", null, "Moderate", "Molar 36" });

            migrationBuilder.InsertData(
                table: "TreatmentSteps",
                columns: new[] { "Id", "Cost", "Description", "Status", "StepOrder", "TreatmentPlanId" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 200m, "Composite filling", 0, 1, new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TenantId",
                table: "Appointments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_TenantId",
                table: "Diagnoses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_VisitId",
                table: "Diagnoses",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosisItems_DiagnosisId",
                table: "DiagnosisItems",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_TenantId",
                table: "HealthRecords",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_VisitId",
                table: "HealthRecords",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_OralXrayImages_TenantId",
                table: "OralXrayImages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OralXrayImages_VisitId",
                table: "OralXrayImages",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_TenantId",
                table: "Patients",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId",
                table: "Payments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Code",
                table: "Tenants",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_TenantId",
                table: "Tenants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlans_TenantId",
                table: "TreatmentPlans",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlans_VisitId",
                table: "TreatmentPlans",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentSteps_TreatmentPlanId",
                table: "TreatmentSteps",
                column: "TreatmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientId",
                table: "Visits",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TenantId",
                table: "Visits",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "DiagnosisItems");

            migrationBuilder.DropTable(
                name: "HealthRecords");

            migrationBuilder.DropTable(
                name: "OralXrayImages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TreatmentSteps");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "TreatmentPlans");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
