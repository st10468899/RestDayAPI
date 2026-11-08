using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public UsersController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Unauthorized(new { message = "You must be logged in to view your profile." });

            var user = await _context.Users
                .Where(u => u.UserID == userId)
                .Select(u => new { u.UserID, u.Name, u.Email, u.Role })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User profile not found." });

            return Ok(user);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Unauthorized(new { message = "You must be logged in to update your profile." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            user.Name = dto.Name;
            user.Email = dto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully." });
        }

        [HttpGet("profile/enrolments")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null || role != "Participant")
                return Unauthorized(new { message = "Access denied. Participants only." });

            var enrolments = await _context.Enrolments
                .Include(e => e.Category)
                .ThenInclude(c => c!.Event)
                .Where(e => e.ParticipantID == userId)
                .Select(e => new
                {
                    e.EnrolmentID,
                    e.EnrolmentDate,
                    EventName = e.Category!.Event!.Name,
                    Category = e.Category.CategoryName
                })
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpGet("profile/results")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null || role != "Participant")
                return Unauthorized(new { message = "Access denied. Participants only." });

            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Category)
                .ThenInclude(c => c!.Event)
                .Where(r => r.Enrolment!.ParticipantID == userId)
                .Select(r => new
                {
                    r.ResultID,
                    EventName = r.Enrolment!.Category!.Event!.Name,
                    Category = r.Enrolment.Category.CategoryName,
                    r.FinishTime,
                    r.FinishingPosition
                })
                .ToListAsync();

            return Ok(results);
        }
    }
}