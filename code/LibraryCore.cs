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

            var NewUser = new User() { Login = login, UserType = usertype, Password = CreatePasswordHash(password) };
            context.Users.Add(NewUser);
            context.SaveChanges();
        }


        private string CreatePasswordHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 20000, HashAlgorithmName.SHA512, 32);
            return $"{Convert.ToHexString(salt)};{Convert.ToHexString(hash)}";
        }


        private bool CheckPasswordHash(string password, string hashAndSalt)
        {
            byte[] input_salt = Convert.FromHexString(hashAndSalt.Split(";").First());
            byte[] input_hash = Convert.FromHexString(hashAndSalt.Split(";").Last());
            byte[] output_hash = Rfc2898DeriveBytes.Pbkdf2(password, input_salt, 20000, HashAlgorithmName.SHA512, 32);

            return CryptographicOperations.FixedTimeEquals(output_hash, input_hash);
        }

        public ICollection<Loan> GetLoansByReader(Reader reader)
        {
            if (reader == null)
                return new List<Loan>();
            context.Entry(reader).Collection("Loans").Load();
            return reader.Loans;
        }

        public (User?, string) EnteredUserIsCorrect(string Login, string password)
        {
            User? user = context.Users.FirstOrDefault(u => u.Login == Login);
            if (user == null)
                return (null, "Логин или пароль введены неправильно.");
            if (!CheckPasswordHash(password, user.Password))
                return (null, "Логин или пароль введены неправильно.");
            return (user, "");
        }

        public Reader? GetReaderByUser(User user)
        {
            context.Entry(user).Reference("Reader").Load();
            return user.Reader;
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
