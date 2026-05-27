using MathBattle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathBattle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameController(AppDbContext context) { _context = context; }

        // DTO для прийому даних з JS
        public class GameResultDto
        {
            public string Nickname { get; set; }
            public int Score { get; set; }
            public int DifficultyId { get; set; }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveResult([FromBody] GameResultDto dto)
        {
            // Шукаємо гравця або створюємо нового
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Nickname == dto.Nickname);
            if (player == null)
            {
                player = new Player { Nickname = dto.Nickname };
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
            }

            // Зберігаємо сесію
            var session = new GameSession
            {
                PlayerId = player.Id,
                DifficultyId = dto.DifficultyId,
                Score = dto.Score,
                PlayedAt = DateTime.UtcNow
            };

            _context.GameSessions.Add(session);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var top = await _context.GameSessions
                .Include(s => s.Player)
                .Include(s => s.Difficulty)
                .OrderByDescending(s => s.Score)
                .Take(10)
                .Select(s => new {
                    Nickname = s.Player.Nickname,
                    Score = s.Score,
                    Difficulty = s.Difficulty.Name,
                    Date = s.PlayedAt.ToLocalTime().ToString("dd.MM HH:mm")
                })
                .ToListAsync();

            return Ok(top);
        }
    }
}