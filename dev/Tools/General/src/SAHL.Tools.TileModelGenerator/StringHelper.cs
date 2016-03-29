using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public class StringHelper
    {

        public static string PropertyToWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        newText.Append(' ');
                    }
                }
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string toCamelCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            if (original.Length <= 4)
            {
                return original.ToLower();
            }
            return string.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }
    }
}
