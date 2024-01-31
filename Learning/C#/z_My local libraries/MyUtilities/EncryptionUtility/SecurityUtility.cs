using System;

namespace EncryptionUtility
{
    /// <summary>
    /// Provides methods for encryption and security operations.
    /// </summary>
    public static class SecurityUtility
    {
        /// <summary>
        /// Encrypts the input text using a simple encryption algorithm.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="encryptionKey">The encryption key. Default is 2.</param>
        public static string EncryptText(string text, short encryptionKey = 2)
        {
            char[] charArray = text.ToCharArray();

            // Iterate over each character in the string and add the encryption key to its ASCII code.
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = (char)(charArray[i] + encryptionKey);
            }

            return new string(charArray);
        }

        /// <summary>
        /// Decrypts the input text that was encrypted using the EncryptText method.
        /// </summary>
        /// <param name="text">The text to decrypt.</param>
        /// <param name="encryptionKey">The encryption key. Default is 2.</param>
        public static string DecryptText(string text, short encryptionKey = 2)
        {
            char[] charArray = text.ToCharArray();

            // Iterate over each character in the string and subtract the encryption key from its ASCII code.
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = (char)(charArray[i] - encryptionKey);
            }

            return new string(charArray);
        }

        /// <summary>
        /// Generates a secure random key for encryption.
        /// </summary>
        /// <param name="keyLength">The length of the random key. Default is 16.</param>
        public static string GenerateRandomKey(int keyLength = 16)
        {
            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=";

            var random = new Random();
            var key = new char[keyLength];

            for (var i = 0; i < keyLength; i++)
            {
                key[i] = Characters[random.Next(Characters.Length)];
            }

            return new string(key);
        }
    }
}
