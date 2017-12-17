using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
namespace P01_BillsPaymentSystem.Data.Models.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(e => e.BankAccountId);
            builder.Ignore(e => e.PaymentMethodId);
            builder.Property(e => e.BankName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(e => e.SwiftCode)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
} 

