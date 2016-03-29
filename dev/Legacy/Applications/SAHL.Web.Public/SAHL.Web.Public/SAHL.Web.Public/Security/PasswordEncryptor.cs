using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace SAHL.Web.Public.Security
{
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
    }
}