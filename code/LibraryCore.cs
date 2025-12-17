using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    internal class LibraryCore
    {
        private LibraryContext context;


        public LibraryCore(string server_name,string user_name, string password)
        {
            context = new LibraryContext(server_name, user_name, password);
        }

        public LibraryCore(string server_name)
        {
            context = new LibraryContext(server_name);
        }

        public void RegisterNewUser(string login, string password, string usertype)
        {

            var NewUser = new User() { Login = login, UserType = usertype, Password = CreatePasswordHash(password)};
            context.RegisterNewUser(NewUser);
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

        public ICollection<Loan> GetLoansByReaderId(int id)
        {
            return context.GetLoansByReaderId(id);
        }

        public (bool,string) EnterUserIsCorrect(string Login, string password)
        {
            User user = context.GetUserByLogin(Login);
            if (user == null)
                return (false, "Логин или пароль введены неправильно.");
            if (!CheckPasswordHash(password, user.Password))
                return (false, "Логин или пароль введены неправильно.");
            return (true, "");


        }
    }
}
