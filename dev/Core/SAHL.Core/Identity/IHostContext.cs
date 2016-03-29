using System;
using System.Security.Principal;

namespace SAHL.Core.Identity
{
    public interface IHostContext
    {
        void IssueSecurityToken(Guid token);

        void SetUser(IIdentity userIdentity, string[] roles);

        IPrincipal GetUser();

        void RevokeSecurityToken();

        string[] GetKeys();

        string[] GetKeysWithPrefix(string keyPrefix);

        string GetContextValue(string contextValueKey, string keyPrefix);
    }
}