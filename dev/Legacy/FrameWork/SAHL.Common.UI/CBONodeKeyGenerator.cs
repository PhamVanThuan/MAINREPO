using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI
{
    internal static class CBONodeKeyGenerator
    {
        private static int _currentKey = 0;

        public static int GetNextKey()
        {
            return ++_currentKey;
        }
    }
}
