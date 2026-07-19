using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PsySchedule.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PsySchedule.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password, string salt)
        {
            byte[] bytesToHash = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            var byteResult = Rfc2898DeriveBytes.Pbkdf2(bytesToHash, saltBytes, 10000, HashAlgorithmName.SHA256 , 64);

            return Convert.ToBase64String(byteResult);
        }

        public bool Verify(string password, string passwordHash, string salt)
        {
            string hash = Hash(password, salt);

            return passwordHash.Equals(hash);
        }
    }
}
