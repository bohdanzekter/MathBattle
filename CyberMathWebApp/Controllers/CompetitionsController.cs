using MathBattle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathBattle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CompetitionsController(AppDbContext context) { _context = context; }

        // GET: Отримати всі схвалені рівні
        [HttpGet]
        public async Task<IActionResult> GetAllApproved()
        {
            var comps = await _context.Competitions
                .Include(c => c.Equations)
                .Where(c => c.IsApproved == true) // ТІЛЬКИ СХВАЛЕНІ АДМІНОМ!
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return Ok(comps);
        }

        public class CreateCompDto
        {
            public string Name { get; set; }
            public int TimeLimit { get; set; }
            public List<EqDto> Equations { get; set; }
        }
        public class EqDto { public string Expression { get; set; } public int Answer { get; set; } }

        // POST: Зберегти новий кастомний рівень
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompDto dto)
        {
            var comp = new Competition { Name = dto.Name, TimeLimitSeconds = dto.TimeLimit };
            _context.Competitions.Add(comp);
            await _context.SaveChangesAsync();

            var eqs = dto.Equations.Select((e, index) => new UserEquation
            {
                CompetitionId = comp.Id,
                Expression = e.Expression,
                Answer = e.Answer,
                OrderIndex = index + 1
            }).ToList();

            _context.UserEquations.AddRange(eqs);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}