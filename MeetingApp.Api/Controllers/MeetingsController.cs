using MeetingApp.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public MeetingsController(MeetingAppContext context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [HttpGet]
        public IEnumerable<Meeting> GetMeeting()
        {
            return _context.Meeting;
        }

        // GET: api/Meetings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeeting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meeting = await _context.Meeting.FindAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        // PUT: api/Meetings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting([FromRoute] int id, [FromBody] Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meeting.Id)
            {
                return BadRequest();
            }

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Meetings
        [HttpPost]
        public async Task<IActionResult> PostMeeting([FromBody] Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Meeting.Add(meeting);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MeetingExists(meeting.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMeeting", new { id = meeting.Id }, meeting);
        }

        // DELETE: api/Meetings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meeting = await _context.Meeting.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meeting.Remove(meeting);
            await _context.SaveChangesAsync();

            return Ok(meeting);
        }

        private bool MeetingExists(int id)
        {
            return _context.Meeting.Any(e => e.Id == id);
        }
    }
}