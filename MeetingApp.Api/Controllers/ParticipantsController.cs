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
    public class ParticipantsController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public ParticipantsController(MeetingAppContext context)
        {
            _context = context;
        }

        // GET: api/Participants
        //[HttpGet]
        //public IEnumerable<Participant> GetParticipant()
        //{
        //    return _context.Participant;
        //}


        // GET: api/Participants
        [HttpGet]
        public IEnumerable<Participant> GetParticipant([FromQuery]string uid, [FromQuery]string mid)
        {
            //‰ï‹cŠÖŒW‚È‚­‚·‚×‚Ä‚ÌŽQ‰ÁŽÒî•ñ‚ð•Ô‚·ê‡
            if (uid == null && mid == null) { return _context.Participant; }

            //ƒ†[ƒU[‚ðŽw’è‚µ‚Ä•Ô‚·ê‡
            else if (uid != null && mid == null)
            {
                var integerUid = int.Parse(uid);

                var participant = _context.Participant.Where(p => p.Uid == integerUid).FirstOrDefault();
                return new Participant[] { participant };
            }
            //‰ï‹cID‚ðŽw’è‚µ‚ÄŽw’è‰ï‹c‚ÌŽQ‰ÁŽÒ‚ð•Ô‚·ê‡
            else if (uid == null && mid != null)
            {
                var integerMid = int.Parse(mid);

                var participants = _context.Participant.Where(p => p.Mid == integerMid);
                return participants;
            }
            //‰ï‹cID‚ðŽw’è‚µ‚ÄŽw’è‰ï‹c‚ÌŽQ‰ÁŽÒ‚ð•Ô‚·ê‡
            else if (uid != null && mid != null)
            {
                var integerMid = int.Parse(mid);
                var integerUid = int.Parse(uid);

                var participants = _context.Participant.Where(p => p.Uid == integerUid && p.Mid == integerMid);
                return participants;
            }
            return null;
        }

        // GET: api/Participants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipant([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participant = await _context.Participant.FindAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }

        // PUT: api/Participants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant([FromRoute] int id, [FromBody] Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.Id)
            {
                return BadRequest();
            }

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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


        [HttpPut]
        public async Task<IActionResult> PutParticipant([FromBody] Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(participant.Uid, participant.Mid))
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

        // POST: api/Participants
        [HttpPost]
        public async Task<IActionResult> PostParticipant([FromBody] Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Participant.Add(participant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipant", new { id = participant.Id }, participant);
        }

        // DELETE: api/Participants/5
        [HttpDelete]
        public async Task<IActionResult> DeleteParticipant([FromQuery] string uid, string mid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var integerUid = int.Parse(uid);
            var integerMid = int.Parse(mid);

            var participant = _context.Participant.Where(p => p.Uid == integerUid && p.Mid == integerMid).FirstOrDefault();

            if (participant == null)
            {
                return NotFound();
            }

            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();

            return Ok(participant);
        }



        private bool ParticipantExists(int id)
        {
            return _context.Participant.Any(e => e.Id == id);
        }

        private bool ParticipantExists(int uid, int mid)
        {
            return _context.Participant.Any(e => e.Uid == uid && e.Mid == mid);
        }
    }
}