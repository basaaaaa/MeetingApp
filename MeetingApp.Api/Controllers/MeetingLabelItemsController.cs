using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingApp.Api.Models;

namespace MeetingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingLabelItemsController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public MeetingLabelItemsController(MeetingAppContext context)
        {
            _context = context;
        }

        // GET: api/MeetingLabelItems
        [HttpGet]
        public IEnumerable<MeetingLabelItem> GetMeetingLabelItem()
        {
            return _context.MeetingLabelItem;
        }

        // GET: api/MeetingLabelItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeetingLabelItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingLabelItem = await _context.MeetingLabelItem.FindAsync(id);

            if (meetingLabelItem == null)
            {
                return NotFound();
            }

            return Ok(meetingLabelItem);
        }

        // PUT: api/MeetingLabelItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingLabelItem([FromRoute] int id, [FromBody] MeetingLabelItem meetingLabelItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meetingLabelItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(meetingLabelItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingLabelItemExists(id))
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

        // POST: api/MeetingLabelItems
        [HttpPost]
        public async Task<IActionResult> PostMeetingLabelItem([FromBody] MeetingLabelItem meetingLabelItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MeetingLabelItem.Add(meetingLabelItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetingLabelItem", new { id = meetingLabelItem.Id }, meetingLabelItem);
        }

        // DELETE: api/MeetingLabelItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingLabelItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingLabelItem = await _context.MeetingLabelItem.FindAsync(id);
            if (meetingLabelItem == null)
            {
                return NotFound();
            }

            _context.MeetingLabelItem.Remove(meetingLabelItem);
            await _context.SaveChangesAsync();

            return Ok(meetingLabelItem);
        }

        private bool MeetingLabelItemExists(int id)
        {
            return _context.MeetingLabelItem.Any(e => e.Id == id);
        }
    }
}