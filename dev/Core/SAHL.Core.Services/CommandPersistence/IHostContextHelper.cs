using SAHL.Core.Identity;
using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface IHostContextHelper
    {
        IHostContext CreateHostContextFromUser(string forUsername, IEnumerable<KeyValuePair<string, string>> contextDetails);

        IEnumerable<string> GetRolesForUsername(string username);
    }
}