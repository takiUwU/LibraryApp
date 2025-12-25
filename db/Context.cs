using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserType> UserTypes { get; set; }

    public string ServerName = "";
    public string ServerUserName = "";
    public string ServerPassword = "";


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(ServerUserName))
            optionsBuilder.UseSqlServer($"SERVER={ServerName};Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=true;", sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(2), errorNumbersToAdd: null); });
        else
            optionsBuilder.UseSqlServer($"SERVER={ServerName};Database=LibraryAppDB;TrustServerCertificate=true;User Id={ServerUserName};Password={ServerPassword};", sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(2), errorNumbersToAdd: null); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorID)
            .OnDelete(DeleteBehavior.Restrict);

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

        modelBuilder.Entity<Reader>()
            .HasIndex(r => r.Phone)
            .IsUnique();
    }
}

