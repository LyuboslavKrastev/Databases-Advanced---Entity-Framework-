using System;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
namespace P01_BillsPaymentSystem
{
    class Startup
    {
        static void Main(string[] args)
        {
            var BillsCont = new BillsPaymentSysContext();
            //BillsCont.Database.EnsureCreated();
            //BillsCont.Database.Migrate();
            //Seed(BillsCont);
            Console.Write("Please Enter a User Id number: ");
            var userId = int.Parse(Console.ReadLine()); 

            if (BillsCont.Users.Find(userId)==null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                return;
            }

            var user = BillsCont.Users.Where(u => u.UserId == userId).Select(e => new
            {
                Name = $"{e.FirstName} {e.LastName}",
                ID = e.UserId,
                CreditCards = e.PaymentMethods.Where(pm => pm.Type == PaymentMethodType.CreditCard)
                .Select(pm => pm.CreditCard).ToArray(),
                BankAccounts = e.PaymentMethods.Where(pm => pm.Type == PaymentMethodType.BankAccount)
                .Select(pm => pm.BankAccount).ToArray()
            }).SingleOrDefault();
            var creditCards = user.CreditCards;
            var bankAccounts = user.BankAccounts;
            Console.WriteLine($"User: {user.Name}");
            GetPaymentMethods(creditCards, bankAccounts);
            PayBills();
        }

        private static void GetPaymentMethods(CreditCard[] creditCards, BankAccount[] bankAccounts)
        {
            if (bankAccounts.Length > 0)
            {
                Console.WriteLine($"Bank Accounts: ");
                foreach (var ba in bankAccounts)
                {
                    Console.WriteLine($"--{ba.BankAccountId}");
                    Console.WriteLine($"---Balance: {ba.Balance:f2}");
                    Console.WriteLine($"---Bank: {ba.BankName}");
                    Console.WriteLine($"---SWIFT: {ba.SwiftCode}");
                }
            }
            if (creditCards.Length > 0)
            {
                Console.WriteLine("Credit Cards:");
                foreach (var cc in creditCards)
                {
                    Console.WriteLine($"--{cc.CreditCardId}");
                    Console.WriteLine($"---Limit: {cc.Limit:f2}");
                    Console.WriteLine($"---MoneyOwed: {cc.MoneyOwed:f2}");
                    Console.WriteLine($"---Limit Left: {cc.LimitLeft:f2}");
                    Console.WriteLine($"---Expiration Date: {cc.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");

                }
            }
        }

        static void PayBills()
        {
            var pbContext = new BillsPaymentSysContext();
            
                try
                {
                    Console.WriteLine("*********Bills Payment Console*********");
                    Console.Write("User ID: ");
                    int userId = int.Parse(Console.ReadLine());
                    Console.Write("Payment amount: ");
                    decimal amount = decimal.Parse(Console.ReadLine());

                    var bankAccountsTotal = pbContext.PaymentMethods
                        .Include(pm => pm.BankAccount)
                        .Where(pm => pm.UserId == userId && pm.Type == PaymentMethodType.BankAccount)
                        .Select(pm => pm.BankAccount)
                        .ToList();

                    var cardsTotal = pbContext.PaymentMethods
                        .Include(pm => pm.CreditCard)
                        .Where(pm => pm.UserId == userId && pm.Type == PaymentMethodType.CreditCard)
                        .Select(pm => pm.CreditCard)
                        .ToList();

                    var totalMoney = bankAccountsTotal.Select(ba => ba.Balance).Sum() 
                        + cardsTotal.Select(cc => cc.LimitLeft).Sum();

                    if (totalMoney < amount)
                    {
                        throw new Exception("Insufficient Funds");
                    }

                    foreach (var account in bankAccountsTotal)
                    {
                        if (amount == 0 || account.Balance == 0)
                        {
                            continue;
                        }

                        decimal moneyInAccount = account.Balance;
                        if (moneyInAccount < amount)
                        {
                            account.Withdraw(moneyInAccount);                      
                        }
                        else
                        {
                            account.Withdraw(amount);
                        Console.WriteLine($"Money left in account: {moneyInAccount - amount}");
                        amount -= amount;
                           

                    }
                }


                    foreach (var creditCard in cardsTotal)
                    {
                        if (amount == 0 || creditCard.LimitLeft == 0)
                        {
                            continue;
                        }

                        decimal limitLeft = creditCard.LimitLeft;
                        if (limitLeft < amount)
                        {
                            creditCard.Withdraw(limitLeft);
                            amount -= limitLeft;
                        }
                        else
                        {
                            creditCard.Withdraw(amount);
                        Console.WriteLine($"Limit left in card: {limitLeft - amount}");
                        amount -= amount;
                        }
                    }

                pbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        

        private static void Seed(BillsPaymentSysContext billsCont)
        {
            var Users = new[] {
            new User()
            {
                FirstName = "McTest",
                LastName = "Testov",
                Email = "McTest@gmail.com",
                Password = "A9uNcAJ"
            },
            new User()
            {
                FirstName = "Test",
                LastName = "McTestov",
                Email = "Test@gmail.com",
                Password = "A9uNcDFAAJ"
            },

            new User()
            {
                FirstName = "Stanka",
                LastName = "Stankova",
                Email = "StankovaS@gmail.com",
                Password = "cDFAA9uNAJ"
            },

            new User()
            {
                FirstName = "Pesho",
                LastName = "Peshev",
                Email = "PeshevP@gmail.com",
                Password = "ANcJD9uFAA"
            }

        };

            billsCont.AddRange(Users);
            var creditCards = new List<CreditCard>()
        {
            new CreditCard()
            {
                Limit = 598.50m,
                MoneyOwed = 40,
                ExpirationDate = DateTime.Parse("07-02-2015")
            },
            new CreditCard()
            {
                Limit = 1298.50m,
                MoneyOwed = 350,
                ExpirationDate = DateTime.Parse("12-10-1998")
            },
            new CreditCard()
            {
                Limit = 598.50m,
                MoneyOwed = 40,
                ExpirationDate = DateTime.Parse("08-07-1979")
            }
        };
            billsCont.AddRange(creditCards);

            var bankAccounts = new []
        {
            new BankAccount()
            {
                Balance = 1459m,
                BankName = "Serious Test Bank",
                SwiftCode = "SprSrsSw"
            },
            new BankAccount()
            {
                Balance = 9876.50m,
                BankName = "Serious Bank Supreme",
                SwiftCode = "SrsBnkSpr"
            }
        };
            billsCont.AddRange(bankAccounts);
            var paymentMethods = new PaymentMethod[]
            {
            new PaymentMethod()
            {
                 User = Users[0],
                 CreditCard = creditCards[2],
                 Type = PaymentMethodType.CreditCard
            },
            new PaymentMethod()
            {
                 User = Users[2],
                 BankAccount = bankAccounts[1],
                 Type = PaymentMethodType.BankAccount
            },
            new PaymentMethod()
            {
                 User = Users[3],
                 CreditCard = creditCards[1],
                 Type = PaymentMethodType.CreditCard
            },
            new PaymentMethod()
            {
                 User = Users[1],
                 BankAccount = bankAccounts[0],
                 Type = PaymentMethodType.BankAccount
            }
            };
            billsCont.AddRange(paymentMethods);

            billsCont.SaveChanges();
        }
    }
}

