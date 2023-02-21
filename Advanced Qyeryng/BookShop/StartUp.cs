namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System;
    using BookShop.Models;
    using System.Drawing;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            //string input = Console.ReadLine();
            int input = int.Parse(Console.ReadLine());
            int result = CountBooks(db, input);
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
            string[] arrayInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            var result = context.Books
                .Select(x => new
                {
                    Categories = x.BookCategories.ToList(),
                    Title = x.Title
                }).OrderBy(b => b.Title).ToList();

            foreach (var book in result)
            {
                foreach (var category in book.Categories)
                {
                    string name = category.Category.Name.ToLower();
                    if (arrayInput.Contains(name))
                    {
                        sb.AppendLine(book.Title);
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        //06 Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateInput = DateTime.Parse(date);
            StringBuilder sb = new StringBuilder();

            var result = context.Books.Where(b => DateTime.Compare(b.ReleaseDate.Value, dateInput) < 0)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEdition = b.EditionType,
                    BookPrice = b.Price
                })
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
                .OrderBy(b=>b.BookId)
                .Where(b=>b.Author.LastName.StartsWith(input))
                .Select(b => new
                {
                    BookTitle = b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
                }).ToList();
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
    }
}
