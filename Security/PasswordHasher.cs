using System;
using System.Security.Cryptography;

namespace LoginWinFormsDb.Security
{
    public static class PasswordHasher
    {
        public static byte[] CreateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static (byte[] hash, byte[] salt, int iterations) HashPassword(string password, int iterations = 100_000)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            var salt = CreateSalt();
            var hash = Pbkdf2(password, salt, iterations);

            return (hash, salt, iterations);
        }

        public static bool Verify(string password, byte[] salt, int iterations, byte[] expectedHash)
        {
            if (password == null || salt == null || expectedHash == null) return false;

            var hash = Pbkdf2(password, salt, iterations);
            return FixedTimeEquals(hash, expectedHash);
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
