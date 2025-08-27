using System;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    public static class StringExtensions
    {
        public const string LettersAndNumbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/";

        public static string GetRandomString(this int length, bool includeSpecialCharacters = false)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be greater than zero.", nameof(length));
            }

            string characterSet = includeSpecialCharacters ? LettersAndNumbers + SpecialCharacters : LettersAndNumbers;
            StringBuilder randomString = new StringBuilder(length);
            byte[] randomBytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            for (int i = 0; i < length; i++)
            {
                randomString.Append(characterSet[randomBytes[i] % characterSet.Length]);
            }

            return randomString.ToString();
        }
    }
}