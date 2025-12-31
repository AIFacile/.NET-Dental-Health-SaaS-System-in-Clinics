using DentalHealthSaaS.Backend.src.Application.DTOs.Auth;
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
    }
}
