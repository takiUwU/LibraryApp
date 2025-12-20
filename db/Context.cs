using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookAmount> BookAmounts { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserType> UserTypes { get; set; }

    //public string ServerName = "";
    //public string ServerUserName = "";
    //public string ServerPassword = "";
    public string ServerName = "26.203.160.220\\SQLEXPRESS,1433";
    public string ServerUserName = "taki";
    public string ServerPassword = "4444";


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(ServerUserName))
            optionsBuilder.UseSqlServer($"SERVER={ServerName};Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=true;", sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null); });
        else
            optionsBuilder.UseSqlServer($"SERVER={ServerName};Database=LibraryAppDB;TrustServerCertificate=true;User Id={ServerUserName};Password={ServerPassword};", sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null); });
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
        .HasOne(u => u.UserType)
        .WithMany(ut => ut.Users)
        .HasForeignKey(u => u.TypeId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        modelBuilder.Entity<BookAmount>().HasKey(ba => ba.BookID);
    }
}

