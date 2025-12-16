using Microsoft.EntityFrameworkCore;

internal class LibraryContext : DbContext
{
    DbSet<Author> Authors { get; set; }
    DbSet<Book> Books { get; set; }
    DbSet<BookAmount> BookAmounts { get; set; }
    DbSet<BookBorrow> BookBorrows { get; set; }
    DbSet<Reader> Readers { get; set; }
    DbSet<User> Users { get; set; }

    private string ServerName = @"26.203.160.220\SQLEXPRESS,1433"; // Пока-что использую просто сервер своего ноутбука. Потом буду делать что-нибудь другое.

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(string.Format("SERVER={0};Database=LibraryAppDB;Trusted_Connection=True;TrustServerCertificate=true;", ServerName));
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

        modelBuilder.Entity<BookBorrow>()
            .HasOne(b => b.Reader)
            .WithMany(r => r.Borrows)
            .HasForeignKey(b => b.ReaderID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookBorrow>()
            .HasOne(b => b.Book)
            .WithMany(b => b.Borrows)
            .HasForeignKey(b => b.BookID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookAmount>().HasKey(ba => ba.BookID);
    }
}

