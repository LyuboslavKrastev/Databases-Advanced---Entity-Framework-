namespace ProductsShop.Data.ModelConfigs
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using ProductsShop.Models;
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p =>p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Price)
              .IsRequired();


            builder.Property(p => p.SellerId)
                .IsRequired();

            builder.HasMany(cp => cp.Categories)
                .WithOne(p => p.Product)
                .HasForeignKey(c => c.ProductId);

            builder.HasOne(p => p.Seller)
                .WithMany(s => s.ProductsSold)
                .HasForeignKey(p => p.SellerId);

            builder.HasOne(p => p.Buyer)
                .WithMany(b => b.ProductsBought)
            .HasForeignKey(p => p.BuyerId);

        }
    }
}

