using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Data.Models;
namespace P01_BillsPaymentSystem.Data.Models.Configurations
{
	public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
	{
		public void Configure(EntityTypeBuilder<CreditCard> builder)
		{
			builder.HasKey(e=>e.CreditCardId);
			builder.Ignore(e=>e.PaymentMethodId);	
			builder.Ignore(e => e.LimitLeft);
					
		}
    } 
}
