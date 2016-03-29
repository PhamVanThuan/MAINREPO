using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Strings;

namespace SAHL.Services.Query.Url
{
    public class UrlParameterSubstituter : IUrlParameterSubstituter
    {
        private readonly IStringReplacer stringReplacer;

        public UrlParameterSubstituter(IStringReplacer stringReplacer)
        {
            this.stringReplacer = stringReplacer;
        }

        public string Replace(string url, IEnumerable<KeyValuePair<string, string>> replacements)
        {
            return stringReplacer.Replace(url, replacements, StringComparison.OrdinalIgnoreCase);
        }
    }
}
