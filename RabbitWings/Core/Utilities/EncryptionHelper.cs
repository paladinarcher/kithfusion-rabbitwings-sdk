using System.Security.Cryptography;
using System.Text;

namespace RabbitWings.Core.Utilities
{
    /// <summary>
    /// Provides methods for password hashing and encryption.
    /// </summary>
    public static class EncryptionHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }
}
