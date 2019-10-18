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
    public class MeetingLabelItemsController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public MeetingLabelItemsController(MeetingAppContext context)
        {
            _context = context;
        }

        //// GET: api/MeetingLabelItems
        //[HttpGet]
        //public IEnumerable<MeetingLabelItem> GetMeetingLabelItem([FromQuery]string lid)
        //{
        //    if (lid == null) { return _context.MeetingLabelItem; }

        //    var integerLid = int.Parse(lid);

        //    return _context.MeetingLabelItem.Where(i => i.Lid == integerLid);

        //}

        // GET: api/MeetingLabelItems
        [HttpGet]
        public IEnumerable<MeetingLabelItem> GetMeetingLabelItem([FromQuery]string uid, [FromQuery]string lid)
        {
            //すべてのラベル項目情報を返す場合
            if (lid == null && uid == null) { return _context.MeetingLabelItem; }

            //特定のユーザーが作成したラベル項目を全て返す場合
            else if (uid != null && lid == null)
            {
                var integerUid = int.Parse(uid);

                var meetingLabelItems = _context.MeetingLabelItem.Where(i => i.Uid == integerUid);
                return meetingLabelItems;
            }
            //指定ラベルに関する項目をユーザー関係なく返す場合
            else if (uid == null && lid != null)
            {
                var integerLid = int.Parse(lid);

                var meetingLabelItems = _context.MeetingLabelItem.Where(i => i.Lid == integerLid);
                return meetingLabelItems;
            }
            //指定ラベルに関する項目を指定ユーザーが作成した分返す場合
            else if (uid != null && lid != null)
            {
                var integerUid = int.Parse(uid);
                var integerLid = int.Parse(lid);

                var meetingLabelItems = _context.MeetingLabelItem.Where(i => i.Uid == integerUid && i.Lid == integerLid);
                return meetingLabelItems;

            }
            return null;
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