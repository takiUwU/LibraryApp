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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryApp
{
    static public class LibraryCore
    {
        static private LibraryContext context = new LibraryContext();

        static public void SetServer(string server_name, string user_name = "", string password = "")
        {
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
            var loans = context.Loans.Where(l => l.Reader == reader).Include(l => l.Book).ToList();
            return loans;
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

        static public (bool,string) TryCreateReader(string phone, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    throw new Exception("Телефон не может быть пустым!");
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Имя не может быть пустым!");
                if (!Regex.Match(phone, @"^(\+[0-9]{9})$").Success)
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
    }
}
