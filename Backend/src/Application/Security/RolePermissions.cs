using DentalHealthSaaS.Backend.src.Domain.Users;

namespace DentalHealthSaaS.Backend.src.Application.Security
{
    public class RolePermissions
    {
        public static readonly Dictionary<RoleName, string[]> Map = new()
        {
            [RoleName.SuperAdmin] =
    [
            "*"
        ],

            [RoleName.TenantAdmin] =
    [
            Permissions.Patients_Read,
            Permissions.Patients_Write,
            Permissions.Visits_Read,
            Permissions.Visits_Write,
            Permissions.Appointments_Read,
            Permissions.Appointments_Write,
            Permissions.Payments_Read,
            Permissions.Payments_Write
        ],

            [RoleName.Doctor] =
    [
            Permissions.Patients_Read,
            Permissions.Visits_Read,
            Permissions.Visits_Write,
            Permissions.Diagnoses_Read,
            Permissions.Diagnoses_Write,
            Permissions.Appointments_Read,
            Permissions.Appointments_Write,
            Permissions.TreatmentPlans_Read,
            Permissions.TreatmentPlans_Write,
            Permissions.HealthRecords_Read,
            Permissions.HealthRecords_Write,
            Permissions.Xrays_Read,
            Permissions.Xrays_Write
        ],

            [RoleName.Assistant] =
    [
            Permissions.Patients_Read,
            Permissions.HealthRecords_Write,
            Permissions.Xrays_Write
        ],

            [RoleName.Receptionist] =
    [
            Permissions.Patients_Read,
            Permissions.Patients_Write,
            Permissions.Appointments_Read,
            Permissions.Appointments_Write
        ],

            [RoleName.Accountant] =
    [
            Permissions.Payments_Read,
            Permissions.Payments_Write
        ]
        };
    }
}
