namespace DentalHealthSaaS.Backend.src.Infrastructure.Data
{
    public class SeedConstants
    {
        // ===== Tenants =====
        public static readonly Guid Tenant1Id =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        // ===== Roles =====
        public static readonly Guid RoleAdminId =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

        public static readonly Guid RoleDoctorId =
            Guid.Parse("33333333-3333-3333-3333-333333333333");

        public static readonly Guid RoleAssistantId =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        // ===== Users =====
        public static readonly Guid AdminUserId =
            Guid.Parse("55555555-5555-5555-5555-555555555555");

        public static readonly Guid DoctorUserId =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        // ===== Patient =====
        public static readonly Guid Patient1Id =
            Guid.Parse("77777777-7777-7777-7777-777777777777");

        // ===== Visit =====
        public static readonly Guid Visit1Id =
            Guid.Parse("88888888-8888-8888-8888-888888888888");

        // ===== Diagnosis =====
        public static readonly Guid Diagnosis1Id =
            Guid.Parse("99999999-9999-9999-9999-999999999999");

        public static readonly Guid DiagnosisItem1Id =
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        // ===== Treatment =====
        public static readonly Guid TreatmentPlan1Id =
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        public static readonly Guid TreatmentStep1Id =
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    }
}
