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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(ServerUserName))
            optionsBuilder.UseSqlServer(string.Format("SERVER={0};Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=true;", ServerName), sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);});
        else
            optionsBuilder.UseSqlServer(string.Format("SERVER={0};Database=LibraryAppDB;TrustServerCertificate=true;User Id={1};Password={2};", ServerName, ServerUserName, ServerPassword), sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null); });
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

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        modelBuilder.Entity<BookAmount>().HasKey(ba => ba.BookID);
    }
}

