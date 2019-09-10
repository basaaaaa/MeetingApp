using MeetingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingLabelsController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public MeetingLabelsController(MeetingAppContext context)
        {
            _context = context;
        }

        // GET: api/MeetingLabels
        //[HttpGet]
        //public IEnumerable<MeetingLabel> GetMeetingLabel()
        //{
        //    return _context.MeetingLabel;
        //}

        // GET: api/MeetingLabels(?mid=~~
        [HttpGet]
        public IEnumerable<MeetingLabel> GetMeetingLabel([FromQuery]string mid)
        {
            if (mid == null) { return _context.MeetingLabel; }

            var integerMid = int.Parse(mid);

            return _context.MeetingLabel.Where(l => l.Mid == integerMid);

        }

        // GET: api/MeetingLabels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeetingLabel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingLabel = await _context.MeetingLabel.FindAsync(id);

            if (meetingLabel == null)
            {
                return NotFound();
            }

            return Ok(meetingLabel);
        }

        // PUT: api/MeetingLabels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingLabel([FromRoute] int id, [FromBody] MeetingLabel meetingLabel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meetingLabel.Id)
            {
                return BadRequest();
            }

            _context.Entry(meetingLabel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingLabelExists(id))
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

        // POST: api/MeetingLabels
        [HttpPost]
        public async Task<IActionResult> PostMeetingLabel([FromBody] MeetingLabel meetingLabel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MeetingLabel.Add(meetingLabel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetingLabel", new { id = meetingLabel.Id }, meetingLabel);
        }

        // DELETE: api/MeetingLabels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingLabel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingLabel = await _context.MeetingLabel.FindAsync(id);
            if (meetingLabel == null)
            {
                return NotFound();
            }

            _context.MeetingLabel.Remove(meetingLabel);
            await _context.SaveChangesAsync();

            return Ok(meetingLabel);
        }

        private bool MeetingLabelExists(int id)
        {
            return _context.MeetingLabel.Any(e => e.Id == id);
        }
    }
}