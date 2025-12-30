using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Users;

namespace DentalHealthSaaS.Backend.src.Domain.Tenants
{
    public class Tenant : BaseEntity
    {
        public required string Code { get; set; }
        public required string Name { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string SubscriptionPlan { get; set; } = "Free";
        public bool IsActive { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
