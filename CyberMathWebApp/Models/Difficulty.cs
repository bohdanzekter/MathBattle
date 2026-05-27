namespace MathBattle.Models
{
    public class Difficulty
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Multiplier { get; set; }
    }
}