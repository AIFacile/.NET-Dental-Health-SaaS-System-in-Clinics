using DentalHealthSaaS.Backend.src.Application.Security;
using DentalHealthSaaS.Backend.src.Domain.Users;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Auth
{
    public class TokenService(IConfiguration config, ApplicationDbContext db) : ITokenService
    {
        private readonly IConfiguration _config = config;
        private readonly ApplicationDbContext _db = db;

        public async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            var permissions = ResolvePermissions(roles);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new("tenant_id", user.TenantId.ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static IReadOnlyCollection<string> ResolvePermissions(
            IReadOnlyCollection<RoleName> roles)
        {
            var permissions = new HashSet<string>();

            foreach (var role in roles)
            {
                if (!RolePermissions.Map.TryGetValue(role, out var rolePermissions))
                    continue;

                // SuperAdmin shortcut
                if (rolePermissions.Contains("*"))
                {
                    return Permissions.All;
                }

                foreach (var permission in rolePermissions)
                {
                    permissions.Add(permission);
                }
            }

            return permissions;
        }
    }
}
