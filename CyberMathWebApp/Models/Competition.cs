namespace MathBattle.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TimeLimitSeconds { get; set; }
        public bool IsApproved { get; set; } = false;

        public List<UserEquation> Equations { get; set; } = new List<UserEquation>();
    }
}