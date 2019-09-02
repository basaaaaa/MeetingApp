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
    public class TokensController : ControllerBase
    {
        private readonly MeetingAppContext _context;

        public TokensController(MeetingAppContext context)
        {
            _context = context;
        }

        //// GET: api/Tokens
        //[HttpGet]
        //public IEnumerable<Token> GetToken()
        //{
        //    return _context.Token;
        //}

        // GET: api/Users
        [HttpGet]
        public IEnumerable<Token> GetToken([FromQuery]string tokenText)
        {
            if (tokenText == null) { return _context.Token; }

            var token = _context.Token.Where(t => t.TokenText == tokenText).FirstOrDefault();
            return new Token[] { token };

        }


        // GET: api/Tokens/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _context.Token.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        // PUT: api/Tokens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken([FromRoute] int id, [FromBody] Token token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != token.Id)
            {
                return BadRequest();
            }

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
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

        // POST: api/Tokens
        [HttpPost]
        public async Task<IActionResult> PostToken([FromBody] Token token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Token.Add(token);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToken", new { id = token.Id }, token);
        }

        // DELETE: api/Tokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _context.Token.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Token.Remove(token);
            await _context.SaveChangesAsync();

            return Ok(token);
        }

        private bool TokenExists(int id)
        {
            return _context.Token.Any(e => e.Id == id);
        }
    }
}