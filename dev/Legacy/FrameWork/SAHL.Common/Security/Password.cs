using System;
using System.Security.Cryptography;
using System.Text;

namespace SAHL.Common.Security
{
    /// <summary>
    /// Utility class for generating and authenticating passwords using hashes
    /// </summary>
    public class Password
    {
        private const int SALTLENGTH = 14;

        /// <summary>
        /// Cryptographically random bytes to enhance hash security
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateSALT(int length)
        {
            byte[] bytes = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Creates an SHA2 hash of a password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] EncryptPassword(string password)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] bytes = encoder.GetBytes(password);
            return new System.Security.Cryptography.SHA384Managed().ComputeHash(bytes);
        }

        /// <summary>
        /// Creates an SHA2 hash of a password with added salt for taste
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] EncryptPassword(string password, byte[] salt)
        {
            string saltStr = Convert.ToBase64String(salt);
            string temp = password + saltStr;
            UTF8Encoding encoder = new UTF8Encoding();
            return new System.Security.Cryptography.SHA384Managed().ComputeHash(encoder.GetBytes(temp));
        }

        /// <summary>
        /// Creates a hash of a password and return it as a string for simplified DB use
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPasswordAsString(string password)
        {
            byte[] bytes = EncryptPassword(password);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Creates a hash of a password with added salt and returns it as a string
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPasswordAsString(string password, byte[] salt)
        {
            byte[] bytes = EncryptPassword(password, salt);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// This ninja-like method will generate a random salt, add it to the password, create a hash and then return a string containing both the hash and the salt
        /// The AuthenticateEmbedded method can be used to validate a password against this bad boy.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPasswordAsStringWithEmbeddedSalt(string password)
        {
            byte[] salt = GenerateSALT(SALTLENGTH);
            string saltStr = Convert.ToBase64String(salt);
            string temp = password + saltStr;
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(encoder.GetBytes(temp));
            string hashStr = Convert.ToBase64String(hash);
            return hashStr + saltStr;
        }

        /// <summary>
        /// Tests the validity of a password against a stored hash
        /// </summary>
        /// <param name="encryptedPassword"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool Authenticate(byte[] encryptedPassword, byte[] storedHash)
        {
            if (encryptedPassword.Length != storedHash.Length)
                return false;

            for (int i = 0; i < encryptedPassword.Length; i++)
            {
                if (encryptedPassword[i] != storedHash[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Tests the validity of a password against a stored hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool Authenticate(string password, byte[] storedHash)
        {
            return Authenticate(EncryptPassword(password), storedHash);
        }

        /// <summary>
        /// Tests the validity of a password against a stored hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool Authenticate(string password, string storedHash)
        {
            string temp = EncryptPasswordAsString(password);
            return (temp == storedHash);
        }

        /// <summary>
        /// Tests the validity of a password against a stored hash with added salt for taste
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool Authenticate(string password, byte[] salt, string storedHash)
        {
            string temp = EncryptPasswordAsString(password, salt);
            return (temp == storedHash);
        }

        /// <summary>
        /// Tests the validity of a password against a stored hash with added salt for taste
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool Authenticate(string password, string salt, string storedHash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            return Authenticate(password, saltBytes, storedHash);
        }

        /// <summary>
        /// Tests the validity of a password created with the EncryptPasswordAsStringWithEmbeddedSalt method
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedEmbeddedHash"></param>
        /// <returns></returns>
        public static bool AuthenticateEmbedded(string password, string storedEmbeddedHash)
        {
            int length = (SALTLENGTH + 2) / 3 * 4;
            int idx = storedEmbeddedHash.Length - length;
            string saltPart = storedEmbeddedHash.Substring(idx, length);
            string hashPart = storedEmbeddedHash.Substring(0, idx);
            string temp = password + saltPart;
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(encoder.GetBytes(temp));
            string hashStr = Convert.ToBase64String(hashBytes);
            return (hashStr == hashPart);
        }
    }
}