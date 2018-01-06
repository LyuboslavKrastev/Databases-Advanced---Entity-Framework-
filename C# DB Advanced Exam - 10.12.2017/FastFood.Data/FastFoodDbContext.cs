using FastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Data
{
	public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}




        protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<Employee>()
                .HasKey(e => e.Id);

            builder.Entity<Position>()
               .HasKey(e => e.Id);


            builder.Entity<Position>()
               .HasAlternateKey(e => e.Name);

            builder.Entity<Category>()
               .HasKey(e => e.Id);

            builder.Entity<Item>()
              .HasKey(e => e.Id);

            builder.Entity<Item>()
         .HasAlternateKey(e => e.Name);

            builder.Entity<Order>()
             .HasKey(e => e.Id);

            builder.Entity<OrderItem>()
             .HasKey(e => new {e.ItemId, e.OrderId });

        }
	}
}