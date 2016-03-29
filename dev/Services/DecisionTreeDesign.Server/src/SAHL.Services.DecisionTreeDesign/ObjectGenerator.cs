using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SAHL.Services.DecisionTreeDesign.Templates
{
    public class ObjectGenerator
    {
       

        public static string StripBadChars(string inString)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(inString, string.Empty);
        }

        public static string ToPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return "";
            }
            original = ObjectGenerator.StripBadChars(original);
            return string.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }

        public static string toCamelCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            original = ObjectGenerator.StripBadChars(original);
            return string.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }

       
    }
}
