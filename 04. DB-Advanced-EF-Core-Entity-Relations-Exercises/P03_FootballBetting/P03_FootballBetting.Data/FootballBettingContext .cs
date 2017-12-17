using System;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
        :base(options){

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(80);

                entity.Property(e => e.Initials)
                .IsRequired()
                .HasColumnType("NCHAR(3)");

                entity.Property(e => e.LogoUrl)
                .IsUnicode(false);

                entity.HasOne(e => e.PrimaryKitColor)
                .WithMany(e => e.PrimaryKitTeams).HasForeignKey(fk => fk.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SecondaryKitColor)
                .WithMany(e => e.SecondaryKitTeams).HasForeignKey(fk => fk.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Town).WithMany(t => t.Teams).HasForeignKey(fk => fk.TownId);           

            });
            builder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.Property(e => e.Name)
                .HasMaxLength(40).IsRequired();

            });

            builder.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.TownId);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(80);

                entity.HasOne(e => e.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(fk => fk.CountryId);

            });


            builder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(80);
            });

            builder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.IsInjured)
                .HasDefaultValue(false);

                entity.HasOne(e => e.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(e => e.TeamId);

                entity.HasOne(e => e.Position)
                .WithMany(t => t.Players)
                .HasForeignKey(e => e.PositionId);  
            });
            builder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId)
                ;

                entity.Property(e => e.Name).
                IsRequired()
                .HasMaxLength(40);

            });

            builder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.GameId });

                entity.HasOne(e => e.Game)
                .WithMany(e => e.PlayerStatistics)
                .HasForeignKey(fk => fk.GameId);

                entity.HasOne(e => e.Player)
                .WithMany(e => e.PlayerStatistics)
                .HasForeignKey(fk => fk.PlayerId);
            });

            builder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.HasOne(e => e.HomeTeam)
                .WithMany(e => e.HomeGames)
                .HasForeignKey(e => e.HomeTeamId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AwayTeam)
                .WithMany(e => e.AwayGames)
                .HasForeignKey(e => e.AwayTeamId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);
                entity.HasOne(e => e.Game)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.User)
                .WithMany(e => e.Bets)
                .HasForeignKey(e => e.UserId);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Name)
                .HasMaxLength(100);

                entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(80);
                entity.Property(e => e.Password)
               .IsRequired()
               .HasMaxLength(80);
                entity.Property(e => e.Email)
               .IsRequired()
               .HasMaxLength(80);

                entity.Property(e => e.Balance)
                .HasDefaultValue(0);
            });
        }
    }
}
