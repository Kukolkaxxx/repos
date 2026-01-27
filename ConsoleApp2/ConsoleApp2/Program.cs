using System.Reflection;

public class Book
{
    public string Title;
    public string Author;
    public int Year;

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Название: {Title}");
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Год: {Year}");
    }

    public class EBook : Book
    {
        public double Filesize { get; set; }
        public EBook(string title, string author, int year, double filesize) : base(title, author, year)
        {
            Filesize = filesize;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Название: {Title}");
            Console.WriteLine($"Автор: {Author}");
            Console.WriteLine($"Год: {Year}");
            Console.WriteLine($"Размер файла: {Filesize}");
        }
    }
}
public class Library
{
    private List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Добавлено");
    }
    public void ListBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("тута ничего нет");
            return;
        }
        Console.WriteLine("список");
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"Книга №1{i + 1}:");
            books[i].DisplayInfo();
        }
    }
    public void Findbook(string title)
    {
        bool find = false;
        foreach (var book in books)
        {
            if (book.Title.ToLower().Contains(title.ToLower()))
            {
                Console.WriteLine("Есть такая!");
            }
        }
        if (!find)
        {
            Console.WriteLine("Тут такого неть(");
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        bool running = true;

        while (running)
        {
            try
            {
                Console.WriteLine("\nМеню");
                Console.WriteLine("1.Добавить книгу");
                Console.WriteLine("2.Ввести список книг");
                Console.WriteLine("3.Найти книгу по названию");
                Console.WriteLine("4.Выход");
                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    Console.Write("Введите тип книги (1 - Обычная, 2 - Электронная): ");
                    int typeChoice = int.Parse(Console.ReadLine());
                    Console.Write("Введите название: .... ");
                    string type = Console.ReadLine();

                    Console.Write("Введите автора: ... ");
                    string author = Console.ReadLine();

                    Console.Write("Введите год издания: ....");
                    int year = int.Parse(Console.ReadLine());

                    if (typeChoice == 1)
                    {
                        Book book = new Book(title, author, year);
                        library.AddBook(book);
                    }
                    else if (typeChoice == 2)
                    {
                        Console.WriteLine("Размер:");
                        double fileSize = double.Parse(Console.ReadLine());
                    }
                }

            }
        }
    }
}