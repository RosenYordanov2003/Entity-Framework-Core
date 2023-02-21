namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System;
    using Microsoft.EntityFrameworkCore;
    using BookShop.Models;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            string input = Console.ReadLine();
            string result = GetBooksReleasedBefore(db, input);
            Console.WriteLine(result);
        }
        //01 Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction;
            bool isEnum = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);
            if (!isEnum)
            {
                return null;
            }
            List<string> books = context.Books.Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                 .OrderBy(t => t).ToList();
            foreach (var bookTitle in books)
            {
                sb.AppendLine(bookTitle);
            }
            return sb.ToString().TrimEnd();
        }
        //02 Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            List<string> books = context.Books.Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold).OrderBy(b => b.BookId)
                .Select(b => b.Title).ToList();
            foreach (var bookTitle in books)
            {
                sb.AppendLine(bookTitle);
            }
            return sb.ToString().TrimEnd();
        }
        //03 Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books.
                Where(b => b.Price > 40)
                .Select(b => new { bookTtile = b.Title, bookPrice = b.Price })
                .OrderByDescending(b => b.bookPrice)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.bookTtile} - ${book.bookPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        //04 Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();
            List<string> books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title).ToList();

            foreach (string bookTitle in books)
            {
                sb.AppendLine(bookTitle);
            }
            return sb.ToString().TrimEnd();
        }
        //05 Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categoryList = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> bookTtiles = new List<string>();

            foreach (string category in categoryList)
            {
                List<string> currentBooks = context.Books
                    .Where(b => b.BookCategories.Any(c => c.Category.Name.ToLower() == category.ToLower()))
                    .Select(b => b.Title).ToList();
                bookTtiles.AddRange(currentBooks);
            }
            bookTtiles = bookTtiles.OrderBy(b => b).ToList();
            return string.Join(Environment.NewLine, bookTtiles);
        }

        //06 Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateInput = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            StringBuilder sb = new StringBuilder();

            var result = context.Books.Where(b => DateTime.Compare(b.ReleaseDate.Value, dateInput) < 0)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEdition = b.EditionType,
                    BookPrice = b.Price,
                    ReleaseDate = b.ReleaseDate,
                })
                   .OrderByDescending(b => b.ReleaseDate)
                .ToList();
            foreach (var book in result)
            {
                sb.AppendLine($"{book.BookTitle} - {book.BookEdition} - ${book.BookPrice:F2}");
            }
            return sb.ToString();
        }

        //07 Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            var authors = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
              .ToList();
            foreach (var author in authors.OrderBy(a => a.FullName))
            {
                sb.AppendLine(author.FullName);
            }
            return sb.ToString().Trim();
        }

        //08 Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            List<string> books = context.Books
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            foreach (string title in books.Where(t => t.Contains(input, StringComparison.OrdinalIgnoreCase)))
            {
                sb.AppendLine(title);
            }
            return sb.ToString().Trim();
        }

        //09 Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Books
                .Where(b => b.Author.LastName.StartsWith(input))
                .Select(b => new
                {
                    BookTitle = b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}",
                    BookId = b.BookId
                }).OrderBy(b => b.BookId).ToList();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.AuthorFullName})");
            }
            return sb.ToString().Trim();
        }
        //10 Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books.Where(b => b.Title.Length > lengthCheck)
                .Count();
            return booksCount;
        }
        //11 Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var authorsCopies = context.Authors
                .Select(a => new
                {
                    AuthorFullName = $"{a.FirstName} {a.LastName}",
                    BookCopies = a.Books.Sum(b => b.Copies)
                }).ToList()
                .OrderByDescending(a => a.BookCopies)
                .ToList();
            foreach (var author in authorsCopies)
            {
                sb.AppendLine($"{author.AuthorFullName} - {author.BookCopies}");
            }
            return sb.ToString().Trim();
        }
        //12 Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CategoryTotalNetWorth = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                }).ToList()
                .OrderByDescending(c => c.CategoryTotalNetWorth)
                .ThenBy(c => c.CategoryName);

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.CategoryName} ${category.CategoryTotalNetWorth}");
            }

            return sb.ToString().Trim();
        }

        //13 Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    RecentBooks = c.CategoryBooks.OrderByDescending(c => c.Book.ReleaseDate).Take(3)
                    .Select(b => new
                    {
                        BookTitle = b.Book.Title,
                        BookReleaseDate = b.Book.ReleaseDate,
                    })
                });
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.RecentBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.BookReleaseDate.Value.Year})");
                }
            }
            return sb.ToString().Trim();
        }

        //14 Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            List<Book> books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();
            foreach (var book in books)
            {
                book.Price += 5;
                context.SaveChanges();
            }
        }

        //15 Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            List<Book> booksToDelete = context.Books.Where(b => b.Copies < 4200).ToList();
            context.Books.RemoveRange(booksToDelete);
            context.SaveChanges();
            return booksToDelete.Count;
        }
    }
}
