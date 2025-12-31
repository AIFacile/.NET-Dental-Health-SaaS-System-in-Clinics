using DentalHealthSaaS.Backend.src.Domain.Users;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Auth
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
