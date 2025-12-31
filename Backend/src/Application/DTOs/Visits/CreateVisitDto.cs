namespace DentalHealthSaaS.Backend.src.Application.DTOs.Visits
{
    public class CreateVisitDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitType { get; set; } = "Initial";
        public string? Notes { get; set; }
    }
}
