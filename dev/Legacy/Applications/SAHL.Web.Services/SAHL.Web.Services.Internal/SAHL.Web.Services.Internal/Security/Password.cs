using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;
using System.Security.Cryptography;

namespace SAHL.Web.Services.Internal.Security
{
    [DataContract]
    public static class Password
    {
        /// <summary>
        /// Calculate MD5 Hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }

        /// <summary>
        /// Generate Random Password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandom(int length)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < length; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}