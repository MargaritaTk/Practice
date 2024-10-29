using Lab7;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class Program
{
    private List<Book> books;
    private int maxBooks;

    public Program(int maxBooks)
    {
        this.maxBooks = maxBooks > 0 ? maxBooks : throw new ArgumentException("The number of books must be > 0.");
        books = new List<Book>();
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1 - Add book");
        Console.WriteLine("2 - Display books");
        Console.WriteLine("3 - Find book");
        Console.WriteLine("4 - Delete book");
        Console.WriteLine("5 - Demonstrate behavior");
        Console.WriteLine("6 - Demonstrate static methods");
        Console.WriteLine("7 - Save collection to file");
        Console.WriteLine("8 - Load collection from file");
        Console.WriteLine("9 - Clear collection");
        Console.WriteLine("0 - Exit");
        Console.Write("Choose an option: ");
    }


    static void Main(string[] args)
    {
        Program program = InitializeProgram();

        int option;
        do
        {
            DisplayMenu();
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input can't be empty. Please enter a valid number.");
                option = -1;
                continue;
            }

            if (int.TryParse(input, out option))
            {
                switch (option)
                {
                    case 1:
                        if (program.DisplayBooks().Count >= program.maxBooks)
                        {

                            Console.WriteLine("Maximum number of books reached. It's Maximum.");
                        }
                        else
                        {
                            Console.Write("Enter the book details (Title;Author;Year;Genre;Pages;Rating): ");
                            string bookInput = Console.ReadLine();
                            try
                            {
                                if (program.AddBook(bookInput))
                                {
                                    Console.WriteLine("Book added successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        break;

                    case 2:
                        var books = program.DisplayBooks();
                        if (books.Count == 0)
                        {
                            Console.WriteLine("No books to display.");
                        }
                        else
                        {
                            for (int i = 0; i < books.Count; i++)
                            {
                                Console.WriteLine($"\nBook {i + 1}: {books[i].ToString()}");
                            }
                        }
                        Console.WriteLine($"\nTotal number of books created: {Book.TotalBooksCreated}");
                        break;

                    case 3:
                        FindBookMenu(program);
                        break;

                    case 4:
                        DeleteBookMenu(program);
                        break;

                    case 5:
                        program.DisplayDemonstrationResults();
                        break;

                    case 6:
                        var staticResults = program.DemonstrateStaticMethods();
                        foreach (var result in staticResults)
                        {
                            Console.WriteLine(result);
                        }
                        break;

                    case 7:
                        SaveToFileMenu(program);
                        break;

                    case 8:
                        LoadFromFileMenu(program);
                        break;

                    case 9:
                        program.ClearCollection();
                        Console.WriteLine("Collection cleared.");
                        break;


                    case 0:
                        Console.WriteLine("Exit...");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
                option = -1;
            }
        } while (option != 0);
    }

    private static Program InitializeProgram()
    {
        int maxBooks;
        while (true)
        {
            try
            {
                Console.Write("Enter the maximum number of books you want to store: ");
                maxBooks = int.Parse(Console.ReadLine());

                if (maxBooks <= 0)
                    throw new ArgumentException("The number of books must be > 0.");

                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid number.");
            }
        }

        return new Program(maxBooks);
    }

    public bool AddBook(string input)
    {
        if (books.Count >= maxBooks)
        {
            Console.WriteLine("Maximum number of books reached. Cannot add more books.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input can't be empty.");
        }

        Book newBook = Book.Parse(input);
        books.Add(newBook);
        return true;
    }

    public List<Book> DisplayBooks()
    {
        return books;
    }
    public Book FindBookByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return null;
        }

        var foundBooks = books.FindAll(b => b.Title.Contains(title));
        if (foundBooks.Count == 0)
        {
            Console.WriteLine("No books found by that title.");
            return null;
        }

        return foundBooks.First();
    }
    public Book FindBookByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("Author cannot be empty.");
            return null;
        }


        var foundBooks = books.FindAll(b => b.Author.Contains(author));
        if (foundBooks.Count == 0)
        {
            Console.WriteLine("No books found by that author.");
            return null;
        }

        return foundBooks.First();
    }

    public bool DeleteBookByTitle(string title)
    {
        Book bookToDelete = null;
        foreach (var book in books)
        {
            if (book.Title.Contains(title))
            {
                bookToDelete = book;
                break;
            }
        }

        if (bookToDelete != null)
        {
            books.Remove(bookToDelete);
            return true;
        }
        return false;
    }

    public bool DeleteBookByIndex(int index)
    {
        if (index < 0 || index >= books.Count)
        {
            return false;
        }

        books.RemoveAt(index);
        Book.TotalBooksCreatedChange();

        return true;
    }

    public int TotalBooksCreated()
    {
        return Book.TotalBooksCreated;
    }

    public List<string> DemonstrateBehavior()
    {
        List<string> result = new List<string>();

        if (books.Count == 0)
        {
            result.Add("No books available for demonstration.");
            return result;
        }

        foreach (var book in books)
        {
            if (book != null)
            {
                result.Add(book.ToString());
                result.Add(book.HighlyRated() ? $"{book.Title} has a high rating." : $"{book.Title} has a low rating.");
                result.Add(book.IsOldBook() ? $"{book.Title} is considered old." : $"{book.Title} is not considered old.");
                book.ShowBookAge(false);
                book.ShowBookAge(true);
            }
        }
        return result;
    }

    public void DisplayDemonstrationResults()
    {
        var behaviorResults = DemonstrateBehavior();
        foreach (var result in behaviorResults)
        {
            Console.WriteLine(result);
        }
    }

    public List<string> DemonstrateStaticMethods()
    {
        return new List<string>
        {
            $"Total books created: {Book.TotalBooksCreated}",
            $"Default publisher: {Book.GetDefaultPublisher()}"
        };
    }

    private static void FindBookMenu(Program program)
    {
        if (program.DisplayBooks().Count == 0)
        {
            Console.WriteLine("No books to found.");
            return;
        }

        string choice;
        while (true)
        {
            Console.WriteLine("Search by:");
            Console.WriteLine("1 - Title");
            Console.WriteLine("2 - Author");
            Console.Write("Choose an option: ");
            choice = Console.ReadLine();

            if (choice == "1" || choice == "2")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 1 or 2.");
            }
        }

        if (choice == "1")
        {
            Console.Write("Enter title to search: ");
            string title = Console.ReadLine();
            var foundBook = program.FindBookByTitle(title);
            if (foundBook != null)
            {
                Console.WriteLine($"\nFound Book: {foundBook.ToString()}");
            }
        }
        else if (choice == "2")
        {
            Console.Write("Enter author to search: ");
            string author = Console.ReadLine();
            var foundBook = program.FindBookByAuthor(author);
            if (foundBook != null)
            {
                Console.WriteLine($"\nFound Book: {foundBook.ToString()}");
            }
        }
    }

    private static void DeleteBookMenu(Program program)
    {
        if (program.DisplayBooks().Count == 0)
        {
            Console.WriteLine("No books to delete.");
            return;
        }

        string choice;
        while (true)
        {
            Console.WriteLine("Delete by:");
            Console.WriteLine("1 - Title");
            Console.WriteLine("2 - Index");
            Console.Write("Choose an option: ");
            choice = Console.ReadLine();

            if (choice == "1" || choice == "2")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 1 or 2.");
            }
        }

        if (choice == "1")
        {
            Console.Write("Enter title to delete: ");
            string title = Console.ReadLine();
            if (program.DeleteBookByTitle(title))
            {
                Console.WriteLine("Book deleted successfully.");
            }
            else
            {
                Console.WriteLine("No books found by that title.");
            }
        }
        else if (choice == "2")
        {
            Console.Write("Enter index of the book to delete (1-based): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= program.DisplayBooks().Count)
            {
                if (program.DeleteBookByIndex(index - 1))
                {
                    Console.WriteLine("Book deleted successfully.");
                }
            }
            else
            {
                Console.WriteLine("Invalid index entered.");
            }
        }
    }

        //NEWPART
        public void ClearCollection()
        {
            books.Clear();
        }

    // CSV serialization/deserialization
    public void SaveToCsv(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var book in books)
            {
                writer.WriteLine(book.ToString()); // Впевніться, що ToString() форматовано з використанням обраного символу
            }
        }
    }


    public void LoadFromCsv(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (Book.TryParse(line, out Book book)) // Використання методу TryParse для перевірки коректності
                {
                    books.Add(book);
                }
            }
        }
    }


    // JSON serialization/deserialization
    public void SaveToJson(string filePath)
    {
        var json = JsonConvert.SerializeObject(books, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Ignore // Включити тільки необхідні властивості
        });
        File.WriteAllText(filePath, json);
    }

    public void LoadFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var loadedBooks = JsonConvert.DeserializeObject<List<Book>>(json);
            if (loadedBooks != null)
            {
                books.AddRange(loadedBooks);
            }
        }
    }


    private static void SaveToFileMenu(Program program)
        {
            Console.WriteLine("Choose the format to save the file:");
            Console.WriteLine("1 - Save to CSV");
            Console.WriteLine("2 - Save to JSON");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter the CSV file path: ");
                string filePath = Console.ReadLine();
                program.SaveToCsv(filePath);
                Console.WriteLine("Books saved to CSV.");
            }
            else if (choice == "2")
            {
                Console.Write("Enter the JSON file path: ");
                string filePath = Console.ReadLine();
                program.SaveToJson(filePath);
                Console.WriteLine("Books saved to JSON.");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        private static void LoadFromFileMenu(Program program)
        {
            Console.WriteLine("Choose the format to load the file:");
            Console.WriteLine("1 - Load from CSV");
            Console.WriteLine("2 - Load from JSON");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter the CSV file path: ");
                string filePath = Console.ReadLine();
                program.LoadFromCsv(filePath);
                Console.WriteLine("Books loaded from CSV.");
            }
            else if (choice == "2")
            {
                Console.Write("Enter the JSON file path: ");
                string filePath = Console.ReadLine();
                program.LoadFromJson(filePath);
                Console.WriteLine("Books loaded from JSON.");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
