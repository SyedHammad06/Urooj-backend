using System.Security.Cryptography;
using System.Text;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Convert the password to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Create SHA-256 instance to hash the password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash to a Base64 string for storage
                string hashString = Convert.ToBase64String(hashBytes);
                return hashString;
            }
        }

        // Method to verify if the entered password matches the stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Hash the entered password
            string enteredPasswordHash = HashPassword(enteredPassword);

            // Compare the newly computed hash with the stored hash
            return enteredPasswordHash == storedHash;
        }
    }
}
