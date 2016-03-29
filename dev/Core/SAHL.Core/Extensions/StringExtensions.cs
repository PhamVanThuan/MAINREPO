using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertToCamelCase(this string source)
        {
            return System.Threading.Thread.CurrentThread.
                CurrentCulture.TextInfo.ToTitleCase(source.ToLower());
        }
    }
}
