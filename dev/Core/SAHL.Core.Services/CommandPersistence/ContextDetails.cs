using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public class ContextDetails : IContextDetails
    {
        public string Username { get; set; }

        public List<KeyValuePair<string, string>> ContextValues { get; set; }
    }
}