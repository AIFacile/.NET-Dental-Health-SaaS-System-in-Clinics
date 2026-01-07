using DentalHealthSaaS.Backend.src.Application.Abstractions.Appointments;
using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.DTOs.Appointments;
using DentalHealthSaaS.Backend.src.Application.DTOs.Visits;
using DentalHealthSaaS.Backend.src.Application.Mappings;
using DentalHealthSaaS.Backend.src.Domain.Appointments;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using DentalHealthSaaS.Backend.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthSaaS.Backend.src.Application.Services
{
    public class AppointmentService(ApplicationDbContext db, IUserContext user) : IAppointmentService
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IUserContext _user = user;

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var patient = await _db.Patients
                .FirstOrDefaultAsync(p => p.Id == dto.PatientId)
                ?? throw new Exception("Patient not found.");

            var doctor = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == dto.DoctorId)
                ?? throw new Exception("Doctor not found.");

            if (dto.EndTime <= dto.StartTime)
                throw new Exception("End time must be after start time.");
            if (dto.StartTime <= DateTime.UtcNow)
                throw new Exception("Cannot create appointment in the past.");

            var newStart = dto.StartTime.ToUniversalTime();
            var newEnd = dto.EndTime.ToUniversalTime();

            var hasConflict = await _db.Appointments.AnyAsync(a =>
                a.Id != dto.Id &&
                a.DoctorId == dto.DoctorId &&
                a.Status != AppointmentStatus.Cancelled &&
                dto.StartTime < a.EndTime &&
                dto.EndTime > a.StartTime);

            if (hasConflict) throw new Exception("Doctor already has appointment at this time.");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = AppointmentStatus.Scheduled
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();

            return appointment.ToDto();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetByDoctorAsync()
        {
            var doctorId = _user.UserId;

            return await _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.ToDto())
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetByDoctorTodayAsync()
        {
            var doctorId = _user.UserId;
            var todayStart = DateTime.Today;
            var tomorrowStart = todayStart.AddDays(1);

            return await _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.StartTime >= todayStart &&
                    a.StartTime < tomorrowStart &&
                    a.Status == AppointmentStatus.Scheduled &&
                    a.Status != AppointmentStatus.Cancelled
                )
                .OrderBy(a => a.StartTime)
                .Select(a => a.ToDto())
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetByPatientAsync(Guid patientId)
        {
            var appointmentExists = await _db.Appointments
                .AnyAsync(a => a.PatientId == patientId);

            return await _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .Select(a => a.ToDto())
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetByDateAsync(DateTime date)
        {
            var todayStart = date;
            var tomorrowStart = todayStart.AddDays(1);

            return await _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.StartTime >= todayStart &&
                    a.StartTime < tomorrowStart &&
                    a.Status != AppointmentStatus.Cancelled
                )
                .OrderBy(a => a.StartTime)
                .Select(a => a.ToDto())
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync()
        {
            return await _db.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => a.ToDto())
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateAppointmentDto dto)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new Exception("Appointment doesn't exist.");

            if (appointment.Status is AppointmentStatus.Cancelled
                or AppointmentStatus.Completed)
                throw new Exception("Canoot modify a finished appointment");

            var hasConflict = await _db.Appointments.AnyAsync(a =>
                a.Id != id &&
                a.DoctorId == appointment.DoctorId &&
                a.Status != AppointmentStatus.Cancelled &&
                dto.StartTime < a.EndTime &&
                dto.EndTime > a.StartTime);

            if (hasConflict)
                throw new Exception("Doctor already has appointment at this time.");

            appointment.Status = dto.Status;
            appointment.StartTime = dto.StartTime;
            appointment.EndTime = dto.EndTime;
            
            await _db.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new Exception("Appointment doesn't exist");

            if (appointment.IsDeleted is true ||
                appointment.Status == AppointmentStatus.Cancelled ||
                appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Appointment is cancelled or completed or somehow deleted.");

            appointment.Status = AppointmentStatus.Cancelled;
            
            await _db.SaveChangesAsync();
        }

        public async Task<VisitDto> CheckInAsync(Guid appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new Exception("Appointment doesn't exist.");

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new Exception("Appointment not ready for check-in");

            var visit = new Visit
            {
                Id = Guid.NewGuid(),
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Status = VisitStatus.Open
            };

            appointment.Status = AppointmentStatus.CheckedIn;
            appointment.VisitId = visit.Id;

            _db.Visits.Add(visit);
            await _db.SaveChangesAsync();

            return visit.ToDto();
        }

        public async Task ConfirmAsync(Guid appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new Exception("Appointment doesn't exist.");

            if (appointment.Status != AppointmentStatus.Scheduled)
                throw new Exception("Appointment is not scheduled.");
            
            appointment.Status = AppointmentStatus.Confirmed;

            await _db.SaveChangesAsync();
        }

        public async Task CompleteAsync(Guid appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new Exception("Appointment doesn't exist.");

            if (appointment.Status != AppointmentStatus.CheckedIn)
                throw new Exception("Appointment hasn't been checked-in.");

            appointment.Status = AppointmentStatus.Completed;

            await _db.SaveChangesAsync();
        }

        public async Task NoShowAsync(Guid appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new Exception("Appointment doesn't exist.");

            if (appointment.Status == AppointmentStatus.Completed ||
                appointment.Status == AppointmentStatus.Cancelled ||
                appointment.IsDeleted == true)
                throw new Exception("Patient has shown.");

            appointment.Status = AppointmentStatus.NoShow;

            await _db.SaveChangesAsync();
        }
    }
}
