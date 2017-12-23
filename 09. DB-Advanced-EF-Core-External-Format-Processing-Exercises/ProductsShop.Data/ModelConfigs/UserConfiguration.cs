namespace ProductsShop.Data.ModelConfigs
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using ProductsShop.Models;
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {       
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
              .IsRequired()
              .HasMaxLength(50);

            builder.Property(u => u.Age)
                .IsRequired(false);
        }
    }
}

