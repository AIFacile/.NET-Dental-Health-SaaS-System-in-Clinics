using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.Patients
{
    public class Patient : BaseEntity
    {
        public required string PatientCode { get; set; }

        public required string Name { get; set; }
        public required string Gender { get; set; }
        public required int Age { get; set; }
        public DateTime BirthDate { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? EmergencyContact { get; set; }

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
