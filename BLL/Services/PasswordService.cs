using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class PasswordService
    {
        public string HashPassword(string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder hashBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashBuilder.Append(b.ToString("x2"));
                }

                return hashBuilder.ToString();
            }
        }

        public bool VerifyPassword(string password, string storedHash, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string hashedPassword = HashPassword(password, cancellationToken);
            return hashedPassword == storedHash;
        }
    }
}
