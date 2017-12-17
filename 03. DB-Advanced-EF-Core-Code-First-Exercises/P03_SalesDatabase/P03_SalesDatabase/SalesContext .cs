

namespace P03_SalesDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data.Models;
    using P03_SalesDatabase.Data;
    using System;
    public class SalesContext : DbContext
    {
        public SalesContext()       {}

        public SalesContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Name)
                .IsUnicode()
                .HasMaxLength(100);

                entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasMaxLength(80);

                entity.Property(e => e.CreditCardNumber)
                .IsRequired();

                entity.HasMany(e => e.Sales)
                .WithOne(e => e.Customer)
                .HasForeignKey(e=>e.SaleId);
                


            });

            modelBuilder.Entity<Sale>(entity =>
            {

                entity.HasKey(e => e.SaleId);

                entity.Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Product)
                .WithMany(e => e.Sales)
                .HasForeignKey(p => p.ProductId);

                entity.HasOne(e => e.Store)
                .WithMany(e => e.Sales)
                .HasForeignKey(p => p.StoreId);

                entity.HasOne(e => e.Customer)
                .WithMany(e => e.Sales)
                .HasForeignKey(p => p.CustomerId);


            });

            modelBuilder.Entity<Store>(entity =>
            {

                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);


                entity.HasMany(e => e.Sales)
                .WithOne(e => e.Store)
                .HasForeignKey(p => p.SaleId);
            });

            modelBuilder.Entity<Product>(entity =>
            {

                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

                entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasDefaultValue("No description");

                entity.Property(e => e.Quantity)
                .IsRequired();

                entity.Property(e => e.Price)
                .IsRequired();





                entity.HasMany(e => e.Sales)
                .WithOne(e => e.Product)
                .HasForeignKey(p => p.SaleId);
            });
        }
    }
}
