namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BookShop.Models;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.Seed(db);
                //DbInitializer.ResetDatabase(db);
                // var command = int.Parse(Console.ReadLine());
                Console.WriteLine(RemoveBooks(db)); 

            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var enumNum = -1;
            if (command.ToLower() == "minor")
            {
                enumNum = 0;
            }
            else if (command.ToLower() == "teen")
            {
                enumNum = 1;
            }
            else if (command.ToLower() == "adult")
            {
                enumNum = 2;
            }

            var books = context.Books.Where(b => (int)b.AgeRestriction == enumNum).Select(b => b.Title).OrderBy(b => b).ToList();

            var booksBuilder = new StringBuilder();
            foreach (var b in books)
            {
                booksBuilder.AppendLine(b);
            }

            return booksBuilder.ToString();

        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title).ToArray();
            var bookBuilder = new StringBuilder();
            foreach (var b in books)
            {
                bookBuilder.AppendLine(b);
            }
            return bookBuilder.ToString().Trim();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Price > 40).Select(b => new
            {
                Title = b.Title,
                Price = b.Price
            }).OrderByDescending(b => b.Price).ToList();

            var bookBuilder = new StringBuilder();
            foreach (var b in books)
            {
                var currString = $"{b.Title} - ${b.Price:f2}";
                bookBuilder.AppendLine(currString);
            }
            return bookBuilder.ToString().Trim();

        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId).Select(b => b.Title).ToList();
            var booksBuilder = new StringBuilder();
            foreach (var b in books)
            {
                booksBuilder.AppendLine(b);
            }

            return booksBuilder.ToString().Trim();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            input = input.ToLower();
            var args = input.Split
                (new[] { "\t", " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books.Where(b => b.BookCategories
            .Any(c => args.Contains(c.Category.Name.ToLower()))).Select(b => b.Title).OrderBy(b => b).ToArray();

            var bookBuilder = new StringBuilder();

            foreach (var b in books)
            {
                bookBuilder.AppendLine(b);
            }
            return bookBuilder.ToString().Trim();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books.Where(b => b.ReleaseDate.Value < dateTime).Select(b => new
            {
                Title = b.Title,
                Edition = b.EditionType,
                Price = b.Price,
                Date = b.ReleaseDate
            }).OrderByDescending(b=>b.Date).ToArray();
            var booksBuilder = new StringBuilder();
            foreach (var b in books)
            {
                var currStr = $"{b.Title} - {b.Edition} - ${b.Price:f2}";
                booksBuilder.AppendLine(currStr);
            }
            return booksBuilder.ToString().Trim();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Books.Where(b => b.Author.FirstName.EndsWith(input))
                .Select(b => new
                {
                    FullName = $"{b.Author.FirstName} {b.Author.LastName}"
                }).OrderBy(b => b.FullName).ToArray();
            var result = new StringBuilder();
            foreach (var a in authors.Distinct())
            {
                result.AppendLine(a.FullName);
            }
            return result.ToString().Trim();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();
            var books = context.Books.Where(b => b.Title.ToLower().Contains(input)).Select(b => b.Title)
                .OrderBy(b=>b)
                .ToArray();

            var result = String.Join(Environment.NewLine, books).Trim();
            return result;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            input = input.ToLower();
            var books = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b=>b.BookId)
                .Select(b => new
                {
                    Title = b.Title,
                    Author = $"{b.Author.FirstName} {b.Author.LastName}"
                }).ToArray();
            var result = new StringBuilder();
            foreach (var b in books)
            {
                var currBook = $"{b.Title} ({b.Author})";
                result.AppendLine(currBook);
            }
            return result.ToString().Trim();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books.Where(b => b.Title.Length > lengthCheck).ToArray();

            var result = books.Length;
            return result;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors.Select(a => new
            {
                AuthorName = $"{a.FirstName} {a.LastName}",
                booksCount = a.Books.Select(b => b.Copies).Sum()
            }).OrderByDescending(a => a.booksCount).ToArray();

            var result = new StringBuilder();
            foreach (var a in authors)
            {
                var currAuthor = $"{a.AuthorName} - {a.booksCount}";
                result.AppendLine(currAuthor);
            }
            return result.ToString().Trim();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {

            var categories = context.Categories.Select(c => new
            {
                CategoryName = $"{c.Name}",
                booksProfit = c.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum()
            }).OrderByDescending(c => c.booksProfit).ThenBy(c=>c.CategoryName).ToArray();

            var result = new StringBuilder();
            foreach (var c in categories)
            {
                var currCategory = $"{c.CategoryName} ${c.booksProfit:f2}";
                result.AppendLine(currCategory);
            }
            return result.ToString().Trim();
        }

        public static string  GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c=>c.Name)              
                .Select(c => new
            {
                c.Name,
                books = c.CategoryBooks.Select(cb => cb.Book)
                .OrderByDescending(cb => cb.ReleaseDate).Take(3)
            }).ToArray();

            var result = new StringBuilder();

            foreach (var c in categories)
            {
                result.AppendLine($"--{c.Name}");
                foreach (var b in c.books)
                {
                    result.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }
            return result.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var b in books)
            {
                b.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var toRemove = context.Books.Where(b => b.Copies < 4200);
            var count = toRemove.Count();
            context.RemoveRange(toRemove);
            context.SaveChanges();
            return count;
        }
    }
}
