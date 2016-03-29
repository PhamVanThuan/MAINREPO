using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface IContextDetails
    {
        string Username { get; set; }

        List<KeyValuePair<string, string>> ContextValues { get; set; }
    }
}