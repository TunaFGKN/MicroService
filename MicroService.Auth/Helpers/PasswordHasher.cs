using System.Security.Cryptography;
using System.Text;

namespace MicroService.AuthWebAPI.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            return Hash(password) == hashedPassword;
        }
    }
}
