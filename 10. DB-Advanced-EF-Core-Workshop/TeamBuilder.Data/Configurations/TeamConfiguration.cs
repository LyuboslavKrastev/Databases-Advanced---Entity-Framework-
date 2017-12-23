namespace TeamBuilder.Data.Configurations
{
    using TeamBuilder.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasAlternateKey(e => e.Name);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(32);


            builder.Property(e => e.Acronym)
                .IsRequired()
                .HasColumnType("CHAR(3)");

            builder.HasOne(t => t.Creator)
                .WithMany(c => c.CreatedTeams)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
