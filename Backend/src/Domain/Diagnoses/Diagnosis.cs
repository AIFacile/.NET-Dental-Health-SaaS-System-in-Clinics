using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.Diagnoses
{
    public class Diagnosis : BaseEntity
    {
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public Guid PatientId { get; set; }

        public DateTime DiagnosisDate { get; set; }
        public Guid DoctorId { get; set; }

        public string Status { get; set; } = "Confirmed";
        public string? Summary { get; set; }

        public ICollection<DiagnosisItem> Items { get; set; } = new List<DiagnosisItem>();
    }
}
