using System.Security.Cryptography;
using System.Text;

namespace BookReview.Service
{
    public class Encryptor : IEncryptor
    {
        public string ComputeHash(string password, string salt)
        {
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltAsBytes = Convert.FromBase64String(salt);
            List<byte> passwordWithSaltBytes = new();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltAsBytes);

            var hashbytes = SHA512.Create().ComputeHash(passwordWithSaltBytes.ToArray());
            return Convert.ToBase64String(hashbytes);
        }

        public string GenerateSalt()
        {
            var bytes =  RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes, 0, bytes.Length);

        }
    }
}
