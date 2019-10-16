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
            //会議関係なくすべての参加者情報を返す場合
            if (uid == null && mid == null) { return _context.Participant; }

            //ユーザーを指定して返す場合
            else if (uid != null && mid == null)
            {
                var integerUid = int.Parse(uid);

                var participant = _context.Participant.Where(p => p.Uid == integerUid).FirstOrDefault();
                return new Participant[] { participant };
            }
            //会議IDを指定して指定会議の参加者を返す場合
            else if (uid == null && mid != null)
            {
                var integerMid = int.Parse(mid);

                var participants = _context.Participant.Where(p => p.Mid == integerMid);
                return participants;
            }
            //例外
            else
            {
                return null;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant([FromRoute] int id)
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

            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();

            return Ok(participant);
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participant.Any(e => e.Id == id);
        }
    }
}