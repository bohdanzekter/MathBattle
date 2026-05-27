namespace MathBattle.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = null!;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}