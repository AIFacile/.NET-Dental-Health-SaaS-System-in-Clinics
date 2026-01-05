using DentalHealthSaaS.Backend.src.Domain.Common;
using DentalHealthSaaS.Backend.src.Domain.Patients;
using DentalHealthSaaS.Backend.src.Domain.Visits;

namespace DentalHealthSaaS.Backend.src.Domain.OralXrayImages
{
    public class OralXrayImage : BaseEntity
    {
        public Guid VisitId { get; set; }
        public Visit Visit { get; set; } = null!;

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public required string ImageType { get; set; }
        public required string ImageUrl { get; set; }

        public DateTime TakenAt { get; set; }
        public string? Description { get; set; }
    }
}
