using Microsoft.EntityFrameworkCore;

public class Author
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}


public class Book
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public int AuthorID { get; set; }
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public int PageCount { get; set; }
    public string? ImagePath { get; set; }
    public Author Author { get; set; } = null!;
    public BookAmount? Amount { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}


public class BookAmount
{
    public int BookID { get; set; }
    public int Amount { get; set; }
    public Book Book { get; set; } = null!;
}


public class Loan
{
    public int ID { get; set; }
    public int ReaderID { get; set; }
    public int BookID { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }

    public Reader Reader { get; set; } = null!;
    public Book Book { get; set; } = null!;
}


public class Reader
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public User User { get; set; } = null!;
}

public class User
{
    public int ID { get; set; }
    public string UserType {  get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? ReaderID { get; set; }
    public Reader? Reader { get; set; } = null!;
}