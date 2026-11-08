using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.Include(e => e.Categories).ToListAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events.Include(e => e.Categories).FirstOrDefaultAsync(e => e.EventID == id);
            if (eventItem == null) return NotFound(new { message = "Event not found." });
            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null || role != "Organiser")
                return Unauthorized(new { message = "Only Organisers can create events." });

            var newEvent = new Event
            {
                OrganiserID = userId.Value,
                Name = dto.Name,
                Description = dto.Description,
                EventDate = dto.EventDate,
                Location = dto.Location,
                Distance = dto.Distance,
                EventType = dto.EventType
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Event created successfully.", eventId = newEvent.EventID });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null || role != "Organiser")
                return Unauthorized(new { message = "Only Organisers can update events." });

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound(new { message = "Event not found." });

            eventItem.Name = dto.Name;
            eventItem.Description = dto.Description;
            eventItem.EventDate = dto.EventDate;
            eventItem.Location = dto.Location;
            eventItem.Distance = dto.Distance;
            eventItem.EventType = dto.EventType;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Event updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Organiser")
                return Unauthorized(new { message = "Only Organisers can delete events." });

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound(new { message = "Event not found." });

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Event deleted successfully." });
        }

        [HttpPost("{eventId}/enrolments")]
        public async Task<IActionResult> EnrolInEvent(int eventId, [FromBody] CreateEnrolmentDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null || role != "Participant")
                return Unauthorized(new { message = "Only Participants can enrol in events." });

            var enrolment = new Enrolment
            {
                ParticipantID = userId.Value,
                CategoryID = dto.CategoryId,
                EnrolmentDate = DateTime.UtcNow
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Enrolment created successfully." });
        }

        [HttpGet("{eventId}/enrolments")]
        public async Task<IActionResult> GetEventEnrolments(int eventId)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Organiser")
                return Unauthorized(new { message = "Only Organisers can view event enrolments." });

            var enrolments = await _context.Enrolments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .Where(e => e.Category!.EventID == eventId)
                .Select(e => new
                {
                    e.EnrolmentID,
                    ParticipantName = e.Participant!.Name,
                    ParticipantEmail = e.Participant.Email,
                    CategoryName = e.Category!.CategoryName
                })
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpPost("{eventId}/results")]
        public async Task<IActionResult> RecordResult(int eventId, [FromBody] RecordResultDto dto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Organiser")
                return Unauthorized(new { message = "Only Organisers can record results." });

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.ParticipantID == dto.ParticipantId && e.Category!.EventID == eventId);

            if (enrolment == null)
                return NotFound(new { message = "Participant enrolment not found for this event." });

            var result = new Result
            {
                EnrolmentID = enrolment.EnrolmentID,
                FinishTime = dto.FinishTime,
                FinishingPosition = dto.FinishingPosition
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Result recorded successfully." });
        }

        [HttpGet("{eventId}/results")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Participant)
                .Include(r => r.Enrolment!.Category)
                .Where(r => r.Enrolment!.Category!.EventID == eventId)
                .Select(r => new
                {
                    r.ResultID,
                    ParticipantName = r.Enrolment!.Participant!.Name,
                    Category = r.Enrolment.Category!.CategoryName,
                    r.FinishTime,
                    r.FinishingPosition
                })
                .ToListAsync();

            return Ok(results);
        }
    }
}