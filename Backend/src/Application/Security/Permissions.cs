using System.Reflection;

namespace DentalHealthSaaS.Backend.src.Application.Security
{
    public static class Permissions
    {
        public static readonly IReadOnlyCollection<string> All =
            [.. typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue()!
            .ToString()!)];

        // ===== Patient =====
        public const string Patients_Read = "patients.read";
        public const string Patients_Write = "patients.write";
        public const string Patients_Delete = "patients.delete";

        // ===== Visit =====
        public const string Visits_Read = "visits.read";
        public const string Visits_Write = "visits.write";

        // ===== Diagnosis =====
        public const string Diagnoses_Read = "diagnoses.read";
        public const string Diagnoses_Write = "diagnoses.write";

        // ===== Appointment =====
        public const string Appointments_Read = "appointments.read";
        public const string Appointments_Write = "appointments.write";

        // ===== TreatmentPlan =====
        public const string TreatmentPlans_Read = "treatmentplans.read";
        public const string TreatmentPlans_Write = "treatmentplans.write";

        // ===== HealthRecord =====
        public const string HealthRecords_Read = "healthrecords.read";
        public const string HealthRecords_Write = "healthrecords.write";

        // ===== Payment =====
        public const string Payments_Read = "payments.read";
        public const string Payments_Write = "payments.write";

        // ===== OralXrayImage =====
        public const string Xrays_Read = "xrays.read";
        public const string Xrays_Write = "xrays.write";
    }
}
