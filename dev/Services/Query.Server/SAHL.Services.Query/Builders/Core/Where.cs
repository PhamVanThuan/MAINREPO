using System.Collections.Generic;
using Newtonsoft.Json;

namespace SAHL.Services.Query.Builders.Core
{
    public class Where
    {
        public Where()
        {
            Items = new Dictionary<string, string>();    
        }

        
        public Dictionary<string, string> Items { get; set; }

    }
}