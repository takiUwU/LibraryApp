using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.code
{
    public static class PasswordManager
    {
        static public string CreatePasswordHash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 20000, HashAlgorithmName.SHA512, 32);
            return $"{Convert.ToHexString(salt)};{Convert.ToHexString(hash)}";
        }


        static public bool CheckPasswordHash(string password, string hashAndSalt)
        {
            byte[] input_salt = Convert.FromHexString(hashAndSalt.Split(";").First());
            byte[] input_hash = Convert.FromHexString(hashAndSalt.Split(";").Last());
            byte[] output_hash = Rfc2898DeriveBytes.Pbkdf2(password, input_salt, 20000, HashAlgorithmName.SHA512, 32);

            return CryptographicOperations.FixedTimeEquals(output_hash, input_hash);
        }
    }
}
