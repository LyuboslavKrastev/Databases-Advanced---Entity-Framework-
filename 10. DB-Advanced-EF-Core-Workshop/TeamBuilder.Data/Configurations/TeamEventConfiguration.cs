namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;
    class TeamEventConfiguration : IEntityTypeConfiguration<TeamEvent>
    {
        public void Configure(EntityTypeBuilder<TeamEvent> builder)
        {
            builder.HasKey(e => new { e.TeamId, e.EventId });

            builder.HasOne(te => te.Team)
                .WithMany(t => t.TeamEvents)
                .HasForeignKey(te => te.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(te => te.Event)
                .WithMany(e => e.ParticipatingTeamEvents)
                .HasForeignKey(te => te.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
