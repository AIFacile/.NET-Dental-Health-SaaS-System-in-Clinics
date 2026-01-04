using DentalHealthSaaS.Backend.src.Domain.Users;

namespace DentalHealthSaaS.Backend.src.Application.DTOs.Auth
{
    public class CreateAccountDto
    {
        public required string TenantCode { get; set; }
        public required string Username { get; set; }
        public required string RealName { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public required RoleName ClaimedRole { get; set; }
    }
}
