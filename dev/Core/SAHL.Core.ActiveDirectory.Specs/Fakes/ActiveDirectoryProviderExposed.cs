using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Specs.Fakes
{
    /// <summary>
    /// A wrapper for ActiveDirectoryProvider that exposes addtional properties for testing as the ActiveDirectoryQuery 
    /// returns types directly from ActiveDirectory library that cannot be mocked (e.g. SearchResultCollection)
    /// </summary>
    public class ActiveDirectoryProviderExposed : ActiveDirectoryProvider
    {
        public bool GetObjectSidWasCalled { get; private set; }

        public bool GetRolesWasCalled { get; private set; }

        public bool GetMemberOfInfoWasCalled { get; private set; }

        public bool GetObjectSidViaQueryWasCalled { get; private set; }

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

        public ActiveDirectoryProviderExposed(IActiveDirectoryQueryFactory queryFactory, IActiveDirectoryProviderCache activeDirectoryProviderCache, ICredentials credentials)
            : base(queryFactory, activeDirectoryProviderCache, credentials)
        {
        }
    }
}