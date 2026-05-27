using MathBattle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathBattle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DifficultiesController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> Get() => await _context.Difficulties.ToListAsync();

        [HttpPost]
        public async Task<ActionResult> Post(Difficulty diff)
        {
            _context.Difficulties.Add(diff);
            await _context.SaveChangesAsync();
            return Ok(diff);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Difficulty diff)
        {
            if (id != diff.Id) return BadRequest();
            _context.Entry(diff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var diff = await _context.Difficulties.FindAsync(id);
            if (diff == null) return NotFound();
            _context.Difficulties.Remove(diff);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}