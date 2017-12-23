namespace ProductsShop.Data.ModelConfigs
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using ProductsShop.Models;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);
           
        }
    }
}

