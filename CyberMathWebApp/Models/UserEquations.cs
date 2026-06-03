namespace MathBattle.Models
{
    public class UserEquation
    {
        public int Id { get; set; }
        public string Expression { get; set; } = null!;
        public int Answer { get; set; }
        public int OrderIndex { get; set; }

        public int CompetitionId { get; set; }
        public Competition? Competition { get; set; }
    }
}