namespace Employees.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using Employees.Models;

    public class EmployeesContext : DbContext
    {
        public EmployeesContext()
        {

        }
        public EmployeesContext(DbContextOptions options)
            :base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(e=>
            {
                e.HasKey(entity => entity.Id);

                e.Property(entity => entity.FirstName)
                .IsRequired().
                HasMaxLength(45);

                e.Property(entity => entity.LastName)
                .IsRequired().
                HasMaxLength(45);

                e.Property(entity => entity.Address)
                .HasMaxLength(200);

                e.HasOne(ent => ent.Manager)
                .WithMany(ent => ent.ManagedEmployees)
                .HasForeignKey(ent => ent.ManagerId);
            });
        }
    }
}
