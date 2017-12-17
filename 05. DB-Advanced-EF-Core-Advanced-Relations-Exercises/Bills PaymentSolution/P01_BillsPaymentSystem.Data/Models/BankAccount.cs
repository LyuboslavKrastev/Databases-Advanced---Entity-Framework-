using System;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        private decimal balance;
        public int BankAccountId { get; set; }
        public decimal Balance
        {
            get
            {
                return this.balance;
            }
            set
            {
                this.balance = value;
            }
        }

        public string BankName { get; set; }
        public string SwiftCode { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount>0)
            {
                this.balance += amount;
            }
            else
            {
                throw new ArgumentException("Invalid Deposit Request");
            }          
        }

        public void Withdraw(decimal amount)
        {
            if (this.balance - amount > 0 && amount > 0)
            {
                this.balance -= amount;
            }
            else
            {
                throw new ArgumentException("Invalid Withdraw Amount");
            }
        }
    }
}