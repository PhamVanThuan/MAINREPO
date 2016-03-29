using System.Collections.Concurrent;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory
{
    public interface IActiveDirectoryProviderCache
    {
        ConcurrentDictionary<string, SecurityIdentifier> ObjectSidsByDistinguishedName { get; }
    }
}