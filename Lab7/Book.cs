using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lab7
{
    public class Book
    {
        // Static fields
        private static int totalBooksCreated = 0;
        private static string Publisher = "Me";

        // Static properties
        public static int TotalBooksCreated { get { return totalBooksCreated; } }

        public static void TotalBooksCreatedChange()
        {
            if (totalBooksCreated > 0)
            {
                totalBooksCreated--;
            }
        }

        public static string DefaultPublisher
        {
            get { return Publisher; }
            set { Publisher = value; }
        }

        private string title;
        private string author;
        private int year;
        private Genre bookGenre;
        private int pages;
        private double rating;

        public bool Available { get; private set; } = true;

        public Book(string title, string author, int year, Genre genre, int pages, double rating)
        {
            Title = title;
            Author = author;
            Year = year;
            BookGenre = genre;
            Pages = pages;
            Rating = rating;
            totalBooksCreated++;
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title can't be empty.");
                title = value;
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Author can't be empty.");
                author = value;
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                    throw new ArgumentException("Invalid year. Must be between 1900 and 2024.");
                year = value;
            }
        }

        public Genre BookGenre
        {
            get { return bookGenre; }
            set
            {
                if (!Enum.IsDefined(typeof(Genre), value))
                    throw new ArgumentException("Invalid genre.");
                bookGenre = value;
            }
        }

        public int Pages
        {
            get { return pages; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Number of pages must be > 0.");
                pages = value;
            }
        }

        public double Rating
        {
            get { return rating; }
            set
            {
                if (value < 1.0 || value > 10.0)
                    throw new ArgumentException("Rating must be between 1.0 and 10.0.");
                rating = value;
            }
        }


        public int BookAge
        {
            get { return DateTime.Now.Year - Year; }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Genre: {BookGenre}");
            Console.WriteLine($"Pages: {Pages}");
            Console.WriteLine($"Rating: {Rating}");
            Console.WriteLine($"Available: {Available}");
        }

        public void ShowBookAge(bool detailed)
        {
            if (detailed)
            {
                Console.WriteLine($"The book \"{Title}\" was published in {Year}. It is {BookAge} years old.");
            }
            else
            {
                Console.WriteLine($"The book \"{Title}\" is {BookAge} years old.");
            }
        }

        public bool HighlyRated()
        {
            return Rating > 8.0;
        }

        public bool IsOldBook()
        {
            return BookAge > 30;
        }

        // Static methods
        public static string GetDefaultPublisher()
        {
            return DefaultPublisher;
        }

        public static Book Parse(string bookString)
        {
            string[] parts = bookString.Split(';');

            if (parts.Length != 6)
                throw new FormatException("Input must have 6 parts.");

            string title = parts[0];
            string author = parts[1];
            if (!int.TryParse(parts[2], out int year) || year < 1900 || year > 2024)
                throw new FormatException("Year must be a valid integer between 1900 and 2024.");

            if (!Enum.TryParse(parts[3], true, out Genre genre) || !Enum.IsDefined(typeof(Genre), genre))
                throw new FormatException("Invalid genre.You must enter only \n(Fiction, Fantasy, Mystery, NonFiction or ScienceFiction).");

            if (!int.TryParse(parts[4], out int pages) || pages <= 0)
                throw new FormatException("Number of pages must be a positive integer.");

            if (!double.TryParse(parts[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double rating) || rating < 1.0 || rating > 10.0)
                throw new FormatException("Rating must be a valid decimal number between 1.0 and 10.0.");

            return new Book(title, author, year, genre, pages, rating);
        }

        public static bool TryParse(string s, out Book obj)
        {
            try
            {
                obj = Parse(s);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, $"{Title};{Author};{Year};{BookGenre};{Pages};{Rating}");
        }
    }
}
   

