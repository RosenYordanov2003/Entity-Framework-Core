namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;

    //Db Context, Fluent API and configuration settings
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }
        public FootballBettingContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>().HasKey(ps => new { ps.GameId, ps.PlayerId });
            modelBuilder.Entity<Team>(e =>
            {
                e.HasOne(t => t.PrimaryKitColor)
                  .WithMany(c => c.PrimaryKitTeams)
                  .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Game>(e =>
            {
                e.HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
        public DbSet<Bet> Bets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
