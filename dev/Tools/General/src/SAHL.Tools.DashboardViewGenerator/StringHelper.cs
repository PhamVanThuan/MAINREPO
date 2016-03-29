using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public static class StringHelper
    {
        public static string StringAfterWord(this string input, string word)
        {
            return input.Substring(input.IndexOf(word) + word.Length);
        }

        public static string minifyToLast(this string input)
        {
            return input.Split("\\".ToCharArray()).Select(x => x[0].ToString().ToLower() + x.Substring(1)).Aggregate((x, k) => x + "\\" + k); ;
        }
    }
}
