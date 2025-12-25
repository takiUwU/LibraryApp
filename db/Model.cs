using Microsoft.EntityFrameworkCore;

public class Author
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive {  get; set; } = true;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}


public class Book
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public int AuthorID { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public int PageCount { get; set; }
    public int Amount { get; set; }
    public bool IsActive { get; set; } = true;
    public Author Author { get; set; } = null!;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}



public class Loan
{
    public int ID { get; set; }
    public int ReaderID { get; set; }
    public int BookID { get; set; }
    public int? UserID { get; set; }
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Reader Reader { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}


public class Reader
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}


public class User
{
    public int ID { get; set; }
    public int TypeId {  get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserType UserType { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public ICollection<Loan> Loans = null!;
}


public class UserType
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}