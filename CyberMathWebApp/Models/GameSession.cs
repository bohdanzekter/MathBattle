namespace MathBattle.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int DifficultyId { get; set; }
        public Difficulty? Difficulty { get; set; }
    }
}