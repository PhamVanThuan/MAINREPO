using System;
using System.Collections.Generic;

namespace SAHL.Services.Query.Factories
{
    public static class DictionaryFactory
    {
        public static Dictionary<string, string> CreateCaseInsensitiveDictionary()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        
        public static Dictionary<string, object> CreateCaseInsensitiveObjectDictionary()
        {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        
    }
}