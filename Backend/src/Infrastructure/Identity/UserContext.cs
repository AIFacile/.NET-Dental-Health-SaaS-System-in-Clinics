using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using System.Security.Claims;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Identity
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public UserContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        Guid IUserContext.UserId
        {
            get
            {
                var userIdClaim = _http.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    throw new UnauthorizedAccessException("User is NOT authenticated.");

                return Guid.Parse(userIdClaim.Value);
            }
        }
        string IUserContext.UserName
        {
            get
            {
                var nameClaim = _http.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Name);

                return nameClaim?.Value ?? "System";
            }
        }
    }
}
