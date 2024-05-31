using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace API.Infrastructure.Cryptography
{
    public class PasswordHasher
    {
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 10000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Return the hashed password and the salt
            return (hashed, Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // Convert the base64 strings to byte arrays
            byte[] salt = Convert.FromBase64String(storedSalt);

            // Hash the entered password using the same salt
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Check if the entered password hash matches the stored hash
            return hash == storedHash;
        }
    }
}