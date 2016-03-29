using System.Collections.Generic;

namespace SAHL.Core.ActiveDirectory.Provider
{
    public interface IActiveDirectoryPrincipalProvider : IActiveDirectoryProvider
    {
        IEnumerable<string> GetGroups(string username);
    }
}