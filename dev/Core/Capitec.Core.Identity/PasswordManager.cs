using SAHL.Core.Cryptography;
using System;
using System.Linq;

namespace Capitec.Core.Identity
{
    public class PasswordManager : IPasswordManager
    {
        public const char PasswordHashingIterationCountSeparator = '.';

        public string HashPassword(string password)
        {
            int count = GetIterationsFromYear(GetCurrentYear());
            var result = Crypto.HashPassword(password, count);
            return EncodeIterations(count) + PasswordHashingIterationCountSeparator + result;
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            var hashedPasswordTemp = hashedPassword;
            if (hashedPasswordTemp.Contains(PasswordHashingIterationCountSeparator))
            {
                var parts = hashedPassword.Split(PasswordHashingIterationCountSeparator);

                int count = DecodeIterations(parts[0]);
                if (count <= 0)
                {
                    return false;
                }

                hashedPasswordTemp = parts[1];

                return Crypto.VerifyHashedPassword(hashedPasswordTemp, password, count);
            }
            else
            {
                return Crypto.VerifyHashedPassword(hashedPasswordTemp, password);
            }
        }

        public string EncodeIterations(int count)
        {
            return count.ToString("X");
        }

        public int DecodeIterations(string prefix)
        {
            int val;
            if (Int32.TryParse(prefix, System.Globalization.NumberStyles.HexNumber, null, out val))
            {
                return val;
            }
            return -1;
        }

        public virtual int GetCurrentYear()
        {
            return DateTime.Now.Year;
        }

        // from OWASP : https://www.owasp.org/index.php/Password_Storage_Cheat_Sheet
        private const int StartYear = 2000;

        private const int StartCount = 1000;

        public int GetIterationsFromYear(int year)
        {
            if (year > StartYear)
            {
                var diff = (year - StartYear) / 2;
                var mul = (int)Math.Pow(2, diff);
                int count = StartCount * mul;
                // if we go negative, then we wrapped (expected in year ~2044).
                // Int32.Max is best we can do at this point
                if (count < 0)
                {
                    count = Int32.MaxValue;
                }
                return count;
                
            }
            return StartCount;
        }
    }
}