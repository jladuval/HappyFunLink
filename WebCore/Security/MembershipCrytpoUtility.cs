namespace WebCore.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal static class MembershipCryptoUtility
    {
        private const string Sha1Algorithm = "SHA1";

        private const string Sha256Algorithm = "SHA256";

        private const int TokenSizeInBytes = 16;

        private const int IterationsCount = 1000;

        // default for Rfc2898DeriveBytes
        private const int SubkeyLength = 256 / 8;

        // 256 bits
        private const int SaltSize = 128 / 8;

        public static string GenerateToken()
        {
            var tokenBytes = new byte[TokenSizeInBytes];
            using (var prng = new RNGCryptoServiceProvider())
            {
                prng.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }

        public static string GenerateSalt(int byteLength = SaltSize)
        {
            return Convert.ToBase64String(GenerateSaltInternal(byteLength));
        }

        public static string Hash(string input, string algorithm/* = Sha256Algorithm*/)
        {
            if (null == input)
                throw new ArgumentNullException("input");

            return Hash(Encoding.UTF8.GetBytes(input), algorithm);
        }

        public static string Hash(byte[] input, string algorithm/* = Sha256Algorithm*/)
        {
            if (null == input)
                throw new ArgumentNullException("input");

            if (string.IsNullOrEmpty(algorithm))
                throw ErrorUtility.CreateArgumentNullOrEmptyException("algorithm");

            using (var alg = HashAlgorithm.Create(algorithm))
            {
                var hashData = alg.ComputeHash(input);
                return BinaryToHex(hashData);
            }
        }

        public static string Sha1Hash(string input)
        {
            return Hash(input, Sha1Algorithm);
        }

        public static string Sha256Hash(string input)
        {
            return Hash(input, Sha256Algorithm);
        }

        // HASHED PASSWORD FORMATS
        // Version 0:
        // PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations.
        // (See also: SDL crypto guidelines v5.1, Part III)
        // Format: { 0x00, salt, subkey }
        public static string HashPassword(string password)
        {
            if (null == password)
                throw new ArgumentNullException("password");

            // Produce a version 0 (see comment above) password hash.
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterationsCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(SubkeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (null == hashedPassword)
            {
                throw new ArgumentNullException("hashedPassword");
            }

            if (null == password)
            {
                throw new ArgumentNullException("password");
            }

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Verify a version 0 (see comment above) password hash
            if (hashedPasswordBytes.Length != (1 + SaltSize + SubkeyLength) || hashedPasswordBytes[0] != (byte)0x00)
            {
                // Wrong length or version header.
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, SubkeyLength);
            byte[] generatedSubkey;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, IterationsCount))
            {
                generatedSubkey = deriveBytes.GetBytes(SubkeyLength);
            }

            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        // 128 bits
        private static byte[] GenerateSaltInternal(int byteLength = SaltSize)
        {
            var buf = new byte[byteLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buf);
            }

            return buf;
        }

        private static string BinaryToHex(byte[] data)
        {
            var hex = new char[data.Length * 2];
            for (var iter = 0; iter < data.Length; iter++)
            {
                var hexChar = (byte)(data[iter] >> 4);
                hex[iter * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                hexChar = (byte)(data[iter] & 0xF);
                hex[(iter * 2) + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
            }

            return new string(hex);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i <= a.Length - 1; i++)
            {
                areSame = areSame & (a[i] == b[i]);
            }

            return areSame;
        }
    }
}
