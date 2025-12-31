namespace DentalHealthSaaS.Backend.src.Application.DTOs.Patients
{
    public class CreatePatientDto
    {
        public required string PatientCode { get; set; }

        public required string Name { get; set; }
        public required string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? EmergencyContact { get; set; }
    }
}
