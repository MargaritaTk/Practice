using Lab7;

namespace MSTest_Lab7
{
    [TestClass]
    public class BookMSTest
    {
        [TestMethod]

        public void TestBookCreation_ValidData_ShouldCreateBook()
        {
            // Arrange
            string title = "Test Book";
            string author = "Test Author";
            int year = 2000;
            Genre genre = Genre.Fiction;
            int pages = 300;
            double rating = 9.0;

            // Act
            Book book = new Book(title, author, year, genre, pages, rating);

            // Assert
            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual(year, book.Year);
            Assert.AreEqual(genre, book.BookGenre);
            Assert.AreEqual(pages, book.Pages);
            Assert.AreEqual(rating, book.Rating);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBookCreation_InvalidTitle_ShouldThrowException()
        {
            // Arrange, Act & Assert
            _ = new Book("", "Author", 2000, Genre.Fiction, 300, 9.0);
        }

        [TestMethod]
        public void TestSetAuthor_InvalidAuthor_ShouldThrowException()
        {
            // Arrange
            Book book = new Book("Valid Title", "Valid Author", 2000, Genre.Fiction, 300, 9.0);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => book.Author = "");
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-10)]
        public void TestSetPages_InvalidPages_ShouldThrowException(int invalidPageCount)
        {
            // Arrange
            Book book = new Book("Valid Title", "Valid Author", 2000, Genre.Fiction, 300, 9.0);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => book.Pages = invalidPageCount);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(11)]
        public void TestSetRating_InvalidRating_ShouldThrowException(double invalidRating)
        {
            // Arrange
            Book book = new Book("Valid Title", "Valid Author", 2000, Genre.Fiction, 300, 9.0);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => book.Rating = invalidRating);
        }


        [TestMethod]
        public void TestHighlyRatedBook_ShouldReturnTrue()
        {
            // Arrange
            Book book = new Book("Test Book", "Author", 2000, Genre.Fiction, 300, 9.0);

            // Act
            bool isHighlyRated = book.HighlyRated();

            // Assert
            Assert.IsTrue(isHighlyRated);
        }

        [TestMethod]
        public void TestOldBook_ShouldReturnFalseForNewBook()
        {
            // Arrange
            Book book = new Book("Test Book", "Author", 2020, Genre.Fiction, 300, 9.0);

            // Act
            bool isOld = book.IsOldBook();

            // Assert
            Assert.IsFalse(isOld);
        }

        [TestMethod]
        public void TestStaticMethod_TotalBooksCreated_ShouldReturnCorrectCount()
        {
            // Arrange
            int initialCount = Book.TotalBooksCreated;
            _ = new Book("Test Book 1", "Author", 2000, Genre.Fiction, 300, 9.0);
            _ = new Book("Test Book 2", "Author", 2005, Genre.Fantasy, 400, 8.0);

            // Act
            int totalBooks = Book.TotalBooksCreated;

            // Assert
            Assert.AreEqual(initialCount + 2, totalBooks);
        }

        [TestMethod]
        public void TestSetTitle_InvalidTitle_ShouldThrowException()
        {
            // Arrange
            Book book = new Book("Valid Title", "Author", 2000, Genre.Fiction, 300, 9.0);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => book.Title = "");
        }

        [TestMethod]
        [DataRow(1800)]
        [DataRow(2027)]
        public void TestSetYear_InvalidYear_ShouldThrowException(int invalidYear)
        {
            // Arrange
            Book book = new Book("Title", "Author", 2000, Genre.Fiction, 300, 9.0);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => book.Year = invalidYear);
        }
        [TestMethod]
        [DataRow("Test Title;Test Author;2005;Fiction;300;9.0", "Test Title", "Test Author", 2005, Genre.Fiction, 300, 9.0)]
        [DataRow("Some Title;Some Author;1975;Mystery;278;5.4", "Some Title", "Some Author", 1975, Genre.Mystery, 278, 5.4)]
        public void TestParse_ValidInput_ShouldReturnBook(string input, string expectedTitle, string expectedAuthor, int expectedYear, Genre expectedGenre, int expectedPages, double expectedRating)
        {
            // Act
            Book book = Book.Parse(input);

            // Assert
            Assert.AreEqual(expectedTitle, book.Title);
            Assert.AreEqual(expectedAuthor, book.Author);
            Assert.AreEqual(expectedYear, book.Year);
            Assert.AreEqual(expectedGenre, book.BookGenre);
            Assert.AreEqual(expectedPages, book.Pages);
            Assert.AreEqual(expectedRating, book.Rating);
        }

        [TestMethod]
        [DataRow("Title;Author;Not a Year;Fiction;300;9.0", "Year must be a valid integer between 1900 and 2024.")]
        [DataRow("Title;Author;1888;Fiction;300;9.0", "Year must be a valid integer between 1900 and 2024.")]
        [DataRow("Title;Author;2025;Fiction;300;9.0", "Year must be a valid integer between 1900 and 2024.")]
        [DataRow("Title;Author;2020;InvalidGenre;300;9.0", "Invalid genre.You must enter only \n(Fiction, Fantasy, Mystery, NonFiction or ScienceFiction).")]
        [DataRow("Title;Author;2020;Fiction;0;9.0", "Number of pages must be a positive integer.")]
        [DataRow("Title;Author;2020;Fiction;-5;9.0", "Number of pages must be a positive integer.")]
        [DataRow("Title;Author;2020;Fiction;300;0.5", "Rating must be a valid decimal number between 1.0 and 10.0.")]
        [DataRow("Title;Author;2020;Fiction;300;11.0", "Rating must be a valid decimal number between 1.0 and 10.0.")]
        [DataRow("Title;Author;2020;Fiction;300;9.0;MorePart", "Input must have 6 parts.")]
        [DataRow("Title;Author;2020;Fiction", "Input must have 6 parts.")]
        public void Parse_InvalidInput_ShouldThrowFormatException(string bookString, string expectedMessage)
        {
            // Act & Assert
            Assert.AreEqual(expectedMessage,
          Assert.ThrowsException<FormatException>(() => Book.Parse(bookString)).Message);
        }


        [TestMethod]
        public void TestTotalBooksCreatedChange_ShouldDecreaseTotalBooks()
        {
            // Arrange
            Book book1 = new Book("Title1", "Author1", 2000, Genre.Fiction, 300, 8.0);
            int initialCount = Book.TotalBooksCreated;

            // Act
            Book.TotalBooksCreatedChange();

            // Assert
            int result = Book.TotalBooksCreated;
            Assert.AreEqual(initialCount - 1, result);
        }



        [TestMethod]
        public void TestDefaultPublisher_SetAndGet_ShouldWorkCorrectly()
        {
            // Arrange
            Book.DefaultPublisher = "Me";

            // Act
            string result = Book.DefaultPublisher;

            // Assert
            Assert.AreEqual("Me", result);
        }


        [TestMethod]
        public void TestBookAge_ShouldReturnCorrectAge()
        {
            // Arrange
            Book book = new Book("Test Title", "Test Author", 2000, Genre.Fiction, 300, 8.5);

            // Act
            int bookAge = book.BookAge;

            // Assert
            Assert.AreEqual(DateTime.Now.Year - 2000, bookAge);
        }


        [TestMethod]
        public void TestGetDefaultPublisher_ShouldReturnDefaultPublisher()
        {
            // Act
            string publisher = Book.GetDefaultPublisher();

            // Assert
            Assert.AreEqual("Me", publisher);
        }



        [TestMethod]
        public void TestTryParse_InvalidInput_ShouldReturnFalse()
        {
            // Arrange
            string input = "Invalid Data";

            // Act
            bool result = Book.TryParse(input, out Book book);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(book);
        }


        [TestMethod]
        public void TestToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            Book book = new Book("Test Title", "Test Author", 2000, Genre.Fiction, 300, 8.5);

            // Act
            string result = book.ToString();

            // Assert
            Assert.AreEqual("Test Title;Test Author;2000;Fiction;300;8,5", result);

        }

        //PARTFORTESTPROGRAM

        [TestClass]
        public class ProgramTest
        {

            [TestMethod]
            public void AddBook_ShouldAddBook_WhenValidInput()    // Перевіряє, чи правильно додається книга за валідним форматом даних.
            {
                var program = new Program(5);
                string bookDetails = "Book Title;Author Name;2020;Fiction;200;4.5";
                bool result = program.AddBook(bookDetails);
                Assert.IsTrue(result);
                Assert.AreEqual(1, program.DisplayBooks().Count);
            }

            [TestMethod]
            public void AddBook_ShouldNotAddBook_WhenExceedsMaxBooks()  // Перевіряє, чи не можна додати більше книг, ніж максимум.
            {
                var program = new Program(5);
                for (int i = 0; i < 5; i++)
                {
                    program.AddBook($"Book {i};Author {i};2020;Fiction;200;4.5");
                }

                bool result = program.AddBook("Book 6;Author 6;2020;Fiction;200;4.5");
                Assert.IsFalse(result);
                Assert.AreEqual(5, program.DisplayBooks().Count);
            }


            [TestMethod]
            public void FindBookByTitle_ShouldReturnBook_WhenBookExists()  // Перевіряє, чи правильно знаходиться книга за заголовком, якщо вона існує.
            {
                var program = new Program(5);
                program.AddBook("Test Book;Test Author;2020;Fiction;200;4.5");
                var foundBook = program.FindBookByTitle("Test Book");
                Assert.IsNotNull(foundBook);
                Assert.AreEqual("Test Book", foundBook.Title);
            }

            [TestMethod]
            public void FindBookByTitle_ShouldReturnNull_WhenBookDoesNotExist() // Перевіряє, чи повертається null, коли книга не знайдена за заголовком.
            {
                var program = new Program(5);
                var foundBook = program.FindBookByTitle("Nonexistent Book");
                Assert.IsNull(foundBook);
            }

            [TestMethod]
            public void DeleteBookByTitle_ShouldDeleteBook_WhenBookExists() // Перевіряє, чи правильно видаляється книга за заголовком, якщо вона існує.
            {
                var program = new Program(5);
                program.AddBook("Book to Delete;Author;2020;Mystery;200;4.5");
                bool deleted = program.DeleteBookByTitle("Book to Delete");
                Assert.IsTrue(deleted);
                Assert.AreEqual(0, program.DisplayBooks().Count);
            }

            [TestMethod]
            public void DeleteBookByTitle_ShouldNotDelete_WhenBookDoesNotExist()   // Перевіряє, чи не вдається видалити книгу, яка не існує.
            {
                var program = new Program(5);
                bool deleted = program.DeleteBookByTitle("Nonexistent Book");
                Assert.IsFalse(deleted);
            }

            [TestMethod]
            public void DeleteBookByIndex_ShouldDeleteBook_WhenIndexIsValid()  // Перевіряє, чи правильно видаляється книга за індексом, якщо індекс дійсний.
            {
                var program = new Program(5);
                program.AddBook("Book 1;Author 1;2020;Fiction;200;4.5");
                program.AddBook("Book 2;Author 2;2020;Fiction;200;4.5");
                bool deleted = program.DeleteBookByIndex(0);
                Assert.IsTrue(deleted);
                Assert.AreEqual(1, program.DisplayBooks().Count);
            }

            [TestMethod]
            public void DeleteBookByIndex_ShouldNotDelete_WhenIndexIsInvalid() // Перевіряє, чи не вдається видалити книгу за невірним індексом.
            {
                var program = new Program(5);
                program.AddBook("Book 1;Author 1;2020;Fiction;200;4.5");
                bool deleted = program.DeleteBookByIndex(10); // Невірний індекс
                Assert.IsFalse(deleted);
                Assert.AreEqual(1, program.DisplayBooks().Count); // Книга не має бути видалена
            }

            [TestMethod]
            public void DemonstrateBehavior_ShouldReturnNoBooksMessage_WhenNoBooksAvailable() // Перевіряє, чи повертається повідомлення про відсутність книг, коли книг немає.
            {
                var program = new Program(5);
                var result = program.DemonstrateBehavior();
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("No books available for demonstration.", result[0]);
            }

            [TestMethod]
            public void DemonstrateStaticMethods_ShouldReturnCorrectInfo()  // Перевіряє, чи статичні методи повертають правильну інформацію.
            {
                var program = new Program(5);
                var result = program.DemonstrateStaticMethods();
                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result[0].StartsWith("Total books created:"));
                Assert.IsTrue(result[1].StartsWith("Default publisher:"));
            }
            [TestMethod]
            public void FindBookByAuthor_ShouldReturnBook_WhenAuthorExists()  // Перевіряє, чи правильно знаходиться книга за автором, якщо він існує.
            {
                // Arrange
                var program = new Program(10);
                program.AddBook("Title1;Author1;2021;Fiction;300;4.5");
                program.AddBook("Title2;Author2;2020;Fantasy;400;4.0");

                // Act
                var foundBook = program.FindBookByAuthor("Author1");

                // Assert
                Assert.IsNotNull(foundBook);
                Assert.AreEqual("Title1", foundBook.Title);
            }

            [TestMethod]
            public void FindBookByAuthor_ShouldReturnNull_WhenAuthorDoesNotExist() // Перевіряє, чи повертається null, коли автор не знайдений.
            {
                // Arrange
                var program = new Program(10);
                program.AddBook("Title1;Author1;2021;Fiction;300;4.5");

                // Act
                var foundBook = program.FindBookByAuthor("NonExistingAuthor");

                // Assert
                Assert.IsNull(foundBook);
            }

            [TestMethod]
            public void FindBookByAuthor_ShouldReturnNull_WhenAuthorIsEmpty()  // Перевіряє, чи повертається null, коли запит на автора з порожнім значенням.
            {
                // Arrange
                var program = new Program(10);
                program.AddBook("Title1;Author1;2021;Fiction;300;4.5");

                // Act
                var foundBook = program.FindBookByAuthor("");

                // Assert
                Assert.IsNull(foundBook);
            }

            [TestMethod]
            public void FindBookByAuthor_ShouldReturnNull_WhenBooksAreEmpty() // Перевіряє, чи повертається null, коли немає книг для пошуку.
            {
                // Arrange
                var program = new Program(10);

                // Act
                var foundBook = program.FindBookByAuthor("AnyAuthor");

                // Assert
                Assert.IsNull(foundBook);
            }

            [TestMethod]
            public void AddBook_ShouldThrowException_WhenInputIsEmpty()  // Перевіряє, чи виникає виняток, коли введено порожній рядок.
            {
                var program = new Program(5);
                Assert.ThrowsException<ArgumentException>(() => program.AddBook(""));
            }

            [TestMethod]
            public void DeleteBookByIndex_ShouldNotDelete_WhenIndexIsNegative()  // Перевіряє, чи не вдається видалити книгу за негативним індексом
            {
                var program = new Program(5);
                program.AddBook("Book 1;Author 1;2020;Fiction;200;4.5");
                bool deleted = program.DeleteBookByIndex(-1);
                Assert.IsFalse(deleted);
                Assert.AreEqual(1, program.DisplayBooks().Count);
            }

            [TestMethod]
            public void FindBookByTitle_ShouldReturnNull_WhenTitleIsEmpty()   // Перевіряє, чи повертається null, коли заголовок порожній.
            {
                var program = new Program(5);
                program.AddBook("Some Book;Some Author;2020;Fiction;200;4.5");
                var foundBook = program.FindBookByTitle("");
                Assert.IsNull(foundBook);
            }

            [TestMethod]
            public void DemonstrateBehavior_ShouldHandleBooksWithDifferentRatings()  // Перевіряє, чи правильно обробляються книги з різними рейтингами.
            {
                var program = new Program(5);
                program.AddBook("Book A;Author A;2020;Fiction;200;9.0");
                program.AddBook("Book B;Author B;2021;Fiction;150;3.0");
                var result = program.DemonstrateBehavior();

                Assert.AreEqual(6, result.Count);
                Assert.IsTrue(result.Contains("Book A has a high rating."));
                Assert.IsTrue(result.Contains("Book B has a low rating."));
            }
        }
    }
}
