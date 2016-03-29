using SAHL.Core.ActiveDirectory.Query.Results;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Provider
{
    public interface IActiveDirectoryProvider
    {
        IEnumerable<string> GetRoles(string username);

        IEnumerable<IMemberOfInfo> GetMemberOfInfo(string username, bool includeSecurityIdentifier = false);

        IEnumerable<T> GetMemberOfInfo<T>(string username, Func<string, SecurityIdentifier, T> resultTransformer, bool includeSecurityIdentifier = false);

        SecurityIdentifier GetObjectSid(string distinguisedName);
    }
}