using Microsoft.EntityFrameworkCore;

namespace MathBattle.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Equation> Equations { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<UserEquation> UserEquations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Difficulty>().HasData(
                new Difficulty { Id = 1, Name = "Easy", Multiplier = 1 },
                new Difficulty { Id = 2, Name = "Medium", Multiplier = 2 },
                new Difficulty { Id = 3, Name = "Hard", Multiplier = 3 },
                new Difficulty { Id = 4, Name = "Expert (Integrals)", Multiplier = 5 }
            );

            modelBuilder.Entity<Equation>().HasData(
              new Equation { Id = 1, Expression = "∫ 2x dx (межі: 0..3)", Answer = 9 },
              new Equation { Id = 2, Expression = "d/dx (x³) при x=2", Answer = 8 },
              new Equation { Id = 3, Expression = "∫ 3x² dx (межі: 0..2)", Answer = 12 },
              new Equation { Id = 4, Expression = "log₂(16)", Answer = 4 },
              new Equation { Id = 5, Expression = "√144 + 3²", Answer = 21 },
              new Equation { Id = 6, Expression = "d/dx (5x²) при x=3", Answer = 30 }
          );

            modelBuilder.Entity<Competition>().HasData(
                new Competition { Id = 1, Name = "Boss Fight: Множення", TimeLimitSeconds = 15 }
            );

            modelBuilder.Entity<UserEquation>().HasData(
                new UserEquation { Id = 1, CompetitionId = 1, Expression = "12 * 12", Answer = 144, OrderIndex = 1 },
                new UserEquation { Id = 2, CompetitionId = 1, Expression = "5 * 15", Answer = 75, OrderIndex = 2 },
                new UserEquation { Id = 3, CompetitionId = 1, Expression = "100 / 4", Answer = 25, OrderIndex = 3 }
            );
        }
    }
}