using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAHL.VS.Build.Web
{
    public class LintTokenizer
    {
        public string Value { get; protected set; }

        public bool IsErrorMessage { get; protected set; }

        public int Line { get; protected set; }

        public int Column { get; protected set; }

        public string Message { get; protected set; }

        public LintTokenizer(string value)
        {
            this.Value = Regex.Replace(value, @"\x1B\[([0-9]{1,3}((;[0-9]{1,3})*)?)?[m|K]", "");
            if (this.Value.StartsWith("  "))
            {
                string[] wordArray = this.Value.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                this.IsErrorMessage = true;
                this.Line = Convert.ToInt32(wordArray[1]) - 1;
                this.Column = Convert.ToInt32(wordArray[3]);
                this.Message = wordArray.Skip(4).Aggregate((x, y) => x + " " + y);
            }
        }
    }
}