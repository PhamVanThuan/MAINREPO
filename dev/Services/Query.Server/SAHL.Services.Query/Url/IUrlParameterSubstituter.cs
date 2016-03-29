using System.Collections.Generic;

namespace SAHL.Services.Query.Url
{
    public interface IUrlParameterSubstituter
    {
        string Replace(string url, IEnumerable<KeyValuePair<string, string>> replacements);
    }
}