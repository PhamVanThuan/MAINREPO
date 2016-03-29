using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Common.Helpers
{
    public class DataGenerator
    {
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static string RandomEmailAddress()
        {
            return string.Concat(RandomString(10), "@", RandomString(5), ".com");
        }
    }
}
