using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    public class StringHelpers
    {
        //for more information refer to
        //http://msdn.microsoft.com/en-us/library/x2dbyw72(v=vs.71).aspx
        
        public static string ToCamelCase(string word)
        {
            return char.ToLower(word[0]) + word.Substring(1);
        }

        public static string ToPascalCase(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
    }
}
