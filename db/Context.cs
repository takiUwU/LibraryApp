using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookAmount> BookAmounts { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<User> Users { get; set; }

    public string ServerName = "";
    public string ServerUserName = "";
    public string ServerPassword = "";

    public LibraryContext(string serverName, string Username, string Password)
    {
        ServerName = serverName;
        ServerUserName = Username;
        ServerPassword = Password;
    }

    public LibraryContext(string serverName) 
    {
        ServerName = serverName;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(ServerUserName))
            optionsBuilder.UseSqlServer(string.Format("SERVER={0};Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=true;", ServerName));
        else
            optionsBuilder.UseSqlServer(string.Format("SERVER={0};Database=LibraryAppDB;TrustServerCertificate=true;User Id={1};Password={2};", ServerName, ServerUserName, ServerPassword));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Amount)
            .WithOne(a => a.Book)
            .HasForeignKey<BookAmount>(a => a.BookID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Loan>()
            .HasOne(b => b.Reader)
            .WithMany(r => r.Loans)
            .HasForeignKey(b => b.ReaderID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
            .HasOne(b => b.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(b => b.BookID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookAmount>().HasKey(ba => ba.BookID);
    }
    
    public void RegisterNewUser(User new_user)
    {
        Users.Add(new_user);
    }

    public ICollection<Loan> GetLoansByReaderId(int id)
    {
        Reader currentReader = Readers.FirstOrDefault(u => u.ID == id);
        if (currentReader == null)
            return new List<Loan>();
        return currentReader.Loans;
    }



    public User GetUserByLogin(string login)
    {
        User user = Users.FirstOrDefault(u => u.Login == login);
        if (user == null)
            throw new Exception("Пользователь не найден!");
        return user;
    }
}

