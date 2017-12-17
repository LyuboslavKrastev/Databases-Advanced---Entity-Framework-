namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    public class CreditCard
    {
        private decimal moneyOwed;
        private decimal limit;

        public int CreditCardId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Limit { get { return this.limit; } set { this.limit = value; } }
        public decimal MoneyOwed { get { return this.moneyOwed; } set { this.moneyOwed = value; } }

        public decimal LimitLeft { get { return this.Limit - this.MoneyOwed; } }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }


        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.moneyOwed -= amount;
            }
            else
            {
                throw new Exception("Invalid Deposit Amount");
            }           
        }

        public void Withdraw(decimal amount)
        {
            if (this.LimitLeft > amount && amount > 0)
            {
                this.moneyOwed += amount;               
            }
            else
            {
                throw new Exception("Invalid Withdraw Amount");
            }           
        }
    }
}