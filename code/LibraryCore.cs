using LibraryApp.code;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryApp
{
    static public class LibraryCore
    {
        static private LibraryContext context = new LibraryContext();

        static public void SetServer(string server_name, string? user_name = "", string? password = "")
        {
            if (user_name == null || password == null)
                context = new LibraryContext() { ServerName = server_name };
            else
                context = new LibraryContext() { ServerName = server_name, ServerUserName = user_name, ServerPassword = password };
        }

        static public void RegisterNewUser(string login, string password, string usertype)
        {

            var NewUser = new User() { Login = login, UserType = context.UserTypes.Where(ut => ut.Name == usertype).First(), Password = PasswordManager.CreatePasswordHash(password) };
            context.Users.Add(NewUser);
            context.SaveChanges();
        }

        static public string GetUserRole(User user)
        {
            context.Entry(user).Reference("UserType").Load();
            return user.UserType.Name;
        }

        static public (User?, string) EnteredUserIsCorrect(string Login, string password)
        {
            User? user = context.Users.FirstOrDefault(u => u.Login == Login);
            if (user == null)
                return (null, "Логин или пароль введены неправильно.");
            if (!PasswordManager.CheckPasswordHash(password, user.Password))
                return (null, "Логин или пароль введены неправильно.");
            return (user, "");
        }


        static public ICollection<Loan> GetLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            var loans = context.Loans.Where(l => l.Reader == reader).Include(l => l.Book).ThenInclude(b => b.Author).ToList();
            return loans;
        }

        static public ICollection<Loan> GetUnreturnedLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            var loans = context.Loans.Where(l => l.Reader == reader).Where(l => l.ReturnDate == null).Include(l => l.Book).ThenInclude(b => b.Author).ToList();
            return loans;
        }

        static public ICollection<Loan> GetExpiredLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            return GetUnreturnedLoansByReader(reader).Where(l => (DateTime.Now - l.BorrowDate).Days > 14).ToList();
        }

        static public ICollection<Loan> GetReturnedLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            return GetUnreturnedLoansByReader(reader).Where(l => l.ReturnDate != null).ToList();
        }

        static public Book? GetBookByID(int id)
        {
            Book? book = context.Books.Where(b => b.ID == id).FirstOrDefault();
            if (book == null)
                return null;
            context.Entry(book).Reference("Author").Load();
            context.Entry(book).Reference("Amount").Load();
            context.Entry(book).Collection("Loans").Load();
            return book;
        }

        static public void DeleteReaderById(int id)
        {
            Reader? reader = context.Readers.Where(r => r.ID == id).FirstOrDefault() ?? null;
            if (reader == null)
                return;
            context.Readers.Remove(reader);
            context.SaveChanges();
        }

        static public void DeleteUserById(int id)
        {
            User? User = context.Users.Where(u => u.ID == id).FirstOrDefault() ?? null;
            if (User == null)
                return;
            context.Users.Remove(User);
            context.SaveChanges();
        }

        static public void DeleteLoanById(int id)
        {
            Loan? Loan = context.Loans.Where(l => l.ID == id).FirstOrDefault() ?? null;
            if (Loan == null)
                return;
            context.Loans.Remove(Loan);
            context.SaveChanges();
        }

        static public void DeleteAuthorById(int id)
        {
            Author? author = context.Authors.Where(a => a.ID == id).FirstOrDefault() ?? null;
            if (author == null)
                return;
            context.Authors.Remove(author);
            context.SaveChanges();
        }

        static public void DeleteBookById(int id)
        {
            Book? book = context.Books.Where(b => b.ID == id).FirstOrDefault() ?? null;
            if (book == null)
                return;
            context.Books.Remove(book);
            context.SaveChanges();
        }

        static public Reader? GetReaderByID(int id)
        {
            Reader? reader = context.Readers.Where(r => r.ID == id).FirstOrDefault();
            if (reader == null)
                return null;
            context.Entry(reader).Reference("Loans").Load();
            return reader;
        }



        static public (bool,string) TryCreateReader(string phone, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    throw new Exception("Телефон не может быть пустым!");
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Имя не может быть пустым!");
                if (!Regex.Match(phone, @"^\+\d{9,20}$").Success)
                    throw new Exception("Номер телефона не подходит. Номер телефона надо вводить со знака +.");
                context.Readers.Add(new Reader() { Phone = phone, Name = name });
                context.SaveChanges();
                return (true,"Успешно добавлен!");
            }
            catch (DbUpdateException)
            {
                return (false, "Данный номер телефона уже зарегистрирован.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        static public Reader? FindReaderByPhone(string phone)
        {
            Reader? reader = context.Readers.Where(r => r.Phone == phone).Include(r => r.Loans).ThenInclude(l => l.Book).FirstOrDefault();
            if (reader == null)
                return null;
            
            return reader;
        }


        static public ICollection<Book> GetAllAvailableBooks()
        {
            return context.Books.Include(b => b.Amount).Include(b => b.Author).Where(b => ((b.Amount != null)) && (b.Amount.Amount > 0)).ToList(); 
        }


        static public ICollection<Author> GetAllAuthors()
        {
            return context.Authors.Include(a => a.Books).ToList();
        }


        static public ICollection<User> GetAllUsers()
        {
            return context.Users.Include(u => u.UserType).ToList();
        }


        static public ICollection<Book> GetAllBooks()
        {
            return context.Books.Include(b => b.Amount).Include(b => b.Author).ToList();
        }


        static public ICollection<Loan> GetAllLoans()
        {
            return context.Loans.Include(l => l.Reader).Include(l => l.Book).ThenInclude(b=>b.Author).ToList();
        }


        static public ICollection<Reader> GetAllReaders()
        {
            return context.Readers.Include(r=>r.Loans).ToList();
        }


        static public bool CanReaderLoanABook(Reader reader)
        {
            context.Entry(reader).Collection("Loans").Load();
            int allowed = 5;
            allowed -= reader.Loans.Where(l => l.ReturnDate == null).Count();
            allowed -= reader.Loans.Where(l => l.ReturnDate != null).Where(l => (l.ReturnDate!.Value - l.BorrowDate).TotalDays > 14).Count();
            return  allowed >= 0;
        }


        static public void CreateALoan(Reader reader, Book book)
        {
            BookAmount? book_amount = context.BookAmounts.Where(ba => ba.BookID == book.ID).FirstOrDefault();
            if (book_amount == null)
                throw new Exception("Количество книг не задано.");
            if (book_amount.Amount <= 0)
                throw new Exception("Книг не достаточно");
            book_amount.Amount -= 1;
            Loan new_loan = new Loan() { ReaderID = reader.ID, BookID = book.ID, BorrowDate = DateTime.Now};
            context.Loans.Add(new_loan);
            context.SaveChanges();
        }


        static public void ReturnALoan(Loan loan)
        {
            if (loan == null) throw new Exception("Долг пустой!");
            if (loan.ReturnDate != null) throw new Exception("Долг уже возращён!");
            loan.ReturnDate = DateTime.Now;
            BookAmount? book_amount = context.BookAmounts.Where(ba => ba.BookID == loan.BookID).FirstOrDefault();
            if (book_amount == null) throw new Exception("Ошибка при получении числа книг!");
            book_amount.Amount += 1;
            context.SaveChanges();
        }


        static public Loan? GetLoanByID(int id)
        {
            Loan? loan = context.Loans.Where(l => l.ID == id).FirstOrDefault();
            if (loan == null)
                return null;
            context.Entry(loan).Reference("Book").Load();
            context.Entry(loan).Reference("Reader").Load();
            return loan;
        }

        static public ICollection<Book>? GetBooksByAuthorId(int authorId)
        {
            Author? author = context.Authors.Where(a => a.ID == authorId).Include(a => a.Books).FirstOrDefault();
            if (author == null)
                return null;
            return author.Books;
        }
    }
}
