using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Identity
{
    /// <summary>
    /// Provides access to information about the currently authenticated user within the HTTP context.
    /// </summary>
    /// <remarks>This class retrieves user identity details, such as user ID and user name, from the current
    /// HTTP context. It is typically used in web applications to obtain information about the user making the current
    /// request. If no user is authenticated, attempts to access the user ID will result in an exception.</remarks>
    public class UserContext(IHttpContextAccessor http) : IUserContext
    {
        private readonly IHttpContextAccessor _http = http;

        public Guid UserId
        {
            get
            {
                var userIdClaim = _http.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier);

                return userIdClaim == null 
                    ? throw new UnauthorizedAccessException("User is NOT authenticated.") 
                    : Guid.Parse(userIdClaim.Value);
            }
        }
        public string UserName
        {
            get
            {
                return !HasUser
                    ? throw new UnauthorizedAccessException("User not resolved.")
                    : _http.HttpContext!.User.FindFirst(UserName)!.Value;
            }
        }
        public bool HasUser =>
            _http.HttpContext?
            .User?
            .HasClaim(u => u.Type == JwtRegisteredClaimNames.Sub) == true;
    }
}
