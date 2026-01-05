using DentalHealthSaaS.Backend.src.Application.DTOs.Auth;
using DentalHealthSaaS.Backend.src.Domain.Users;
using DentalHealthSaaS.Backend.src.Infrastructure.Auth;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordHasher = DentalHealthSaaS.Backend.src.Infrastructure.Auth.PasswordHasher;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ITokenService tokenService, ApplicationDbContext db) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly ApplicationDbContext _db = db;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(t => t.Code == dto.TenantCode);

            if (tenant == null) return Unauthorized("Invalid tenant");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => 
                    u.Username == dto.Username &&
                    u.TenantId == tenant.Id);

            if (user == null) return Unauthorized("Invalid username or password.");

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid username or password");

            var token = await _tokenService.GenerateTokenAsync(user);

            return Ok(new AuthResponseDto
            {
                AccessToken = token,
            });
        }

        [HttpPost("create-account")]
        [AllowAnonymous] // only for development
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Code == dto.TenantCode);

            if (tenant == null) return NotFound("Tenant not found.");

            PasswordHasher.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Username = dto.Username,
                RealName = dto.RealName,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsActive = true,
            };

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.ClaimedRole);
            if (role == null) return NotFound("Claimed role name doesn't exist.");
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
            };

            _db.Users.Add(user);
            _db.UserRoles.Add(userRole);

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("add-role")]
        [AllowAnonymous] // only for development
        public async Task <IActionResult> AddRole([FromBody] RoleName roleName)
        {
            var roleNameExists = await _db.Roles.AnyAsync(r => r.Name == roleName);

            if (roleNameExists)
            {
                throw new Exception("Role exists.");
            }

            _db.Roles.Add(new Role
            {
                Id = Guid.NewGuid(),
                Name = roleName
            });

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
