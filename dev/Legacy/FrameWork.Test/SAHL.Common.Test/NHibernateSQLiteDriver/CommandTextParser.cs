using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class CommandTextParser
    {
        public string ParseCommandText(string text)
        {
            if (!text.Contains("."))
                return text;
            text = text.Replace("_.", "_$");
            text = text.Replace(".", "_");
            text = text.Replace("_$", "_.");
            return text;
        }
    }
}