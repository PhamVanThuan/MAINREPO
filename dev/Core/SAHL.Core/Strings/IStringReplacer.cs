using System;
using System.Collections.Generic;

namespace SAHL.Core.Strings
{
    public interface IStringReplacer
    {
        string Replace(string source, IEnumerable<KeyValuePair<string, string>> replacements, StringComparison comparison);
    }
}