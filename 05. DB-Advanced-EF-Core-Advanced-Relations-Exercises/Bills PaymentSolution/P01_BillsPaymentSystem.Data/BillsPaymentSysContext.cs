namespace P01_BillsPaymentSystem.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.Models;
    using P01_BillsPaymentSystem.Data.Models.Configurations;
    public class BillsPaymentSysContext : DbContext
    {
        public BillsPaymentSysContext()
        {

        }

        public BillsPaymentSysContext(DbContextOptions Options)
        : base(Options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new PaymentMethodConfiguration());
            builder.ApplyConfiguration(new BankAccountConfiguration());
            builder.ApplyConfiguration(new CreditCardConfiguration());
        }
    }
}
