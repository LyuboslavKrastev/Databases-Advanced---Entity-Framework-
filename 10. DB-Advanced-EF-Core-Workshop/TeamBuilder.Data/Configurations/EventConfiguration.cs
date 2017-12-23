namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;
    class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired().
                HasMaxLength(25)
                .IsUnicode();

            builder
                .Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(250)
                .IsUnicode();

            builder.Property(e => e.StartDate)
                .IsRequired(false);

            builder.Property(e => e.EndDate)
                .IsRequired(false);

            builder.HasOne(e => e.Creator)
                .WithMany(c => c.CreatedEvents)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
