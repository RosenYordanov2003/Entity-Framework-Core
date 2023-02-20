namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            string result = GetBooksByCategory(db, "horror mystery drama");
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
        //public static string GetBooksByCategory(BookShopContext context, string input)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    //var books = context.Books
        //    //    .Select(b => new { bookTitle = b.Title, bookCategories = b.BookCategories})
        //    //    .OrderBy(t=>t.bookTitle)
        //    //    .ToList();
        //    //sb.Append("Hello");

        //    //var result = books.Where(b=>GetPredicate(b.bookCategories,input)).ToList();
        //    var result = context.Books
        //        .Select(x => new
        //        {
        //            CategoryName = x.BookCategories.Select(c=>c.Category.Name),
        //            Title = x.Title
        //        }).ToList();
        //    foreach (var item in result)
        //    {
        //        sb.AppendLine(item.CategoryName.);
        //    }
        //    return sb.ToString().TrimEnd();
        //}
    }
}
