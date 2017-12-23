namespace TeamBuilder.Data
{
    using Instagraph.Data;
    using Microsoft.EntityFrameworkCore;
    using TeamBuilder.Data.Configurations;
    using TeamBuilder.Models;

    public class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext() {}

        public TeamBuilderContext(DbContextOptions options)
            : base()
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<TeamEvent> TeamEvents { get; set; }

        public DbSet<UserTeam> UserTeams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new TeamConfiguration());

            builder.ApplyConfiguration(new EventConfiguration());

            builder.ApplyConfiguration(new InvitationConfiguration());

            builder.ApplyConfiguration(new UserTeamConfiguration());

            builder.ApplyConfiguration(new TeamEventConfiguration());

        }
    }
}
