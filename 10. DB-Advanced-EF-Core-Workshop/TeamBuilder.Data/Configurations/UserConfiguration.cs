
namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasAlternateKey(e => e.Username);


            builder.Property(e => e.Username)
                .IsRequired(true);

            builder.Property(e => e.Password)
                .IsRequired(true);

            builder.Property(e => e.FirstName)
                .IsRequired(false);

            builder.Property(e => e.LastName)
                .IsRequired(false);
        }
    }
}
