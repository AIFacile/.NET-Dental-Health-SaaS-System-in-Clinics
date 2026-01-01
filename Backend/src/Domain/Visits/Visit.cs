using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Diagnoses;
using DentalHealthSaaS.Backend.src.Domain.HealthRecords;
using DentalHealthSaaS.Backend.src.Domain.OralXrayImages;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Treatments;

namespace DentalHealthSaaS.Backend.src.Domain.Visits
{
    public class Visit : BaseEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public DateTime VisitDate { get; set; }
        public string VisitType { get; set; } = "Initial";
        public VisitStatus Status { get; set; } = VisitStatus.Open;

        public Guid DoctorId { get; set; }

        public string? Notes { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
        public ICollection<TreatmentPlan> TreatmentPlans { get; set; } = new List<TreatmentPlan>();
        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
        public ICollection<OralXrayImage> OralXrayImages { get; set; } = new List<OralXrayImage>();
    }
}
