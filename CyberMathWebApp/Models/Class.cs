namespace MathBattle.Models
{
    public class Equation
    {
        public int Id { get; set; }
        public string Expression { get; set; } = null!;
        public int Answer { get; set; }
    }
}