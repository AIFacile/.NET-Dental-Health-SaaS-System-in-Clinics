namespace DentalHealthSaaS.Backend.src.Application.DTOs.Auth
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TenantCode { get; set; } = null!;
    }
}
