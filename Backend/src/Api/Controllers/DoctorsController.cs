using DentalHealthSaaS.Backend.src.Application.DTOs.Doctors;
using DentalHealthSaaS.Backend.src.Domain.Users;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorsController(ApplicationDbContext db) : ControllerBase
    {
        private readonly ApplicationDbContext _db = db;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DoctorDto>>> GetDoctors()
        {
            var doctors = await _db.UserRoles
                .AsNoTracking()
                .Where(ur => ur.Role.Name == RoleName.Doctor)
                .Select(ur => new DoctorDto
                {
                    Id = ur.User.Id,
                    DoctorName = ur.User.RealName
                })
                .Distinct()
                .ToListAsync();

            return Ok(doctors);
        }
    }

}
