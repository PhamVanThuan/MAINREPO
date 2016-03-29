using System;
using System.Security.Cryptography;

namespace SAHL.Core.Metrics
{
    internal class Random
    {
        private static readonly RandomNumberGenerator _random;

        static Random()
        {
            Random._random = RandomNumberGenerator.Create();
        }

        public static long NextLong()
        {
            byte[] buffer = new byte[8];
            Random._random.GetBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}