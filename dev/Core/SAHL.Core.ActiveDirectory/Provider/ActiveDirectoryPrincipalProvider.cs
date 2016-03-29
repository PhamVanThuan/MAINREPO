using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Query;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace SAHL.Core.ActiveDirectory.Provider
{
    public class ActiveDirectoryPrincipalProvider : ActiveDirectoryProvider, IActiveDirectoryPrincipalProvider
    {
        public ActiveDirectoryPrincipalProvider(IActiveDirectoryQueryFactory queryFactory, IActiveDirectoryProviderCache activeDirectoryProviderCache, ICredentials credentials)
            : base(queryFactory, activeDirectoryProviderCache, credentials)
        {
        }

        /// <summary>
        /// Returns the groups that the specified username belongs to, including nested groups e.g. Domain Users
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetRoles(string username)
        {
            return GetGroups(username);
        }

        public virtual IEnumerable<string> GetGroups(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                yield break;
            }

            using (var context = new PrincipalContext(ContextType.Domain, Environment.UserDomainName, Credentials.Username, Credentials.Password))
            {
                var directoryPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);
                if (directoryPrincipal == null)
                {
                    yield break;
                }
                var groups = directoryPrincipal.GetGroups();
                foreach (var item in groups)
                {
                    yield return item.SamAccountName; //name of the group to which the user belongs
                }
            }
        }
    }
}