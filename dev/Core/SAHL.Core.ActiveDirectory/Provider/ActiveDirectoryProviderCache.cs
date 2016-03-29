using System.Collections.Concurrent;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Provider
{
    public class ActiveDirectoryProviderCache : IActiveDirectoryProviderCache
    {
        public ConcurrentDictionary<string, SecurityIdentifier> ObjectSidsByDistinguishedName { get; private set; }

        public ActiveDirectoryProviderCache()
        {
            ObjectSidsByDistinguishedName = new ConcurrentDictionary<string, SecurityIdentifier>();
        }
    }
}