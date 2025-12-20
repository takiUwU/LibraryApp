using LibraryApp.code;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    public class LibraryCore
    {
        private LibraryContext context;


        public LibraryCore(string server_name, string user_name = "", string password = "")
        {
            context = new LibraryContext() { ServerName = server_name, ServerUserName = user_name, ServerPassword = password };
        }

        public void RegisterNewUser(string login, string password, string usertype)
        {

            var NewUser = new User() { Login = login, UserType = context.UserTypes.Where(ut => ut.Name == usertype).First(), Password = PasswordManager.CreatePasswordHash(password) };
            context.Users.Add(NewUser);
            context.SaveChanges();
        }

        public string GetUserRole(User user)
        {
            context.Entry(user).Reference("UserType").Load();
            return user.UserType.Name;
        }

        public (User?, string) EnteredUserIsCorrect(string Login, string password)
        {
            User? user = context.Users.FirstOrDefault(u => u.Login == Login);
            if (user == null)
                return (null, "Логин или пароль введены неправильно.");
            if (!PasswordManager.CheckPasswordHash(password, user.Password))
                return (null, "Логин или пароль введены неправильно.");
            return (user, "");
        }


        public ICollection<Loan> GetLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            var loans = context.Loans.Where(l => l.Reader == reader).Include(l => l.Book).ToList();
            return loans;
        }

        public Book? GetBookByID(int id)
        {
            Book? book = context.Books.Where(b => b.ID == id).FirstOrDefault();
            if (book == null)
                return null;
            context.Entry(book).Reference("Author").Load();
            context.Entry(book).Reference("Amount").Load();
            context.Entry(book).Collection("Loans").Load();
            return book;
        }
    }
}
