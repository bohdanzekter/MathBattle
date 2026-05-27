using MathBattle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathBattle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EquationsController(AppDbContext context) { _context = context; }

        // GET: api/equations/random
        [HttpGet("batch")]
        public async Task<IActionResult> GetBatch()
        {
            var equations = await _context.Equations
                .Select(e => new { e.Expression, e.Answer })
                .ToListAsync();

            return Ok(equations);
        }
    }
}