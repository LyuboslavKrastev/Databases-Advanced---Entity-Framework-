
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
namespace P01_BillsPaymentSystem.Data.Models.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => new
            {
                e.UserId,
                e.BankAccountId,
                e.CreditCardId
            }).IsUnique(); //StackOverFlow Advice For Creating A Unique while using EF CORE

            builder.HasOne(e => e.User)
                    .WithMany(u => u.PaymentMethods)
                    .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.CreditCard)
                    .WithOne(pm => pm.PaymentMethod)
                    .HasForeignKey<PaymentMethod>(e => e.CreditCardId);

            builder.HasOne(e => e.BankAccount)
            .WithOne(pm => pm.PaymentMethod)
            .HasForeignKey<PaymentMethod>(e => e.BankAccountId);
        }
    }
}
