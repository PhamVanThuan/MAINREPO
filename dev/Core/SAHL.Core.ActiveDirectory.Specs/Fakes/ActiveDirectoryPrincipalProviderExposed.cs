using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Specs.Fakes
{
    public class ActiveDirectoryPrincipalProviderExposed : ActiveDirectoryPrincipalProvider
    {
        public bool GetObjectSidWasCalled { get; private set; }

        public bool GetRolesWasCalled { get; private set; }

        public bool GetMemberOfInfoWasCalled { get; private set; }

        public bool GetObjectSidViaQueryWasCalled { get; private set; }

        public bool GetGroupsWasCalled { get; private set; }

        public ActiveDirectoryPrincipalProviderExposed(IActiveDirectoryQueryFactory queryFactory
            , IActiveDirectoryProviderCache activeDirectoryProviderCache, ICredentials credentials)
            : base(queryFactory, activeDirectoryProviderCache, credentials)
        {
        }

        public override IEnumerable<string> GetGroups(string username)
        {
            GetGroupsWasCalled = true;
            return base.GetGroups(username);
        }

        public override SecurityIdentifier GetObjectSid(string distinguisedName)
        {
            GetObjectSidWasCalled = true;
            return base.GetObjectSid(distinguisedName);
        }

        protected override SecurityIdentifier GetObjectSidViaQuery(string distinguisedName)
        {
            GetObjectSidViaQueryWasCalled = true;
            return base.GetObjectSidViaQuery(distinguisedName);
        }

        public override IEnumerable<string> GetRoles(string username)
        {
            GetRolesWasCalled = true;
            return base.GetRoles(username);
        }

        public override IEnumerable<T> GetMemberOfInfo<T>(string username, Func<string, SecurityIdentifier, T> resultTransformer, bool includeSecurityIdentifier)
        {
            GetMemberOfInfoWasCalled = true;
            return base.GetMemberOfInfo(username, resultTransformer, includeSecurityIdentifier);
        }
    }
}