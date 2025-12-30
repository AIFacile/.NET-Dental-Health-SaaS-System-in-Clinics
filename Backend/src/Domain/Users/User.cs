using DentalHealthSaaS.Backend.src.Domain.Common;

namespace DentalHealthSaaS.Backend.src.Domain.Users
{
    public class User : BaseEntity
    {
        public required string Username { get; set; }
        public required string RealName { get; set; }

        public string? Email { get; set; }

        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
