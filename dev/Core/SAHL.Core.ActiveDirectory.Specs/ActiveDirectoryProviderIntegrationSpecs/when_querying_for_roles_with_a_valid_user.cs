using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Specs.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.ActiveDirectory.Specs.ActiveDirectoryProviderIntegrationSpecs
{
    public class when_querying_for_roles_with_a_valid_user : WithFakes
    {
        private static ActiveDirectoryPrincipalProviderExposed ActiveDirectoryProvider;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;
        private static List<string> Roles;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = new ActiveDirectoryProviderCache();

            ActiveDirectoryQueryFactory = new ActiveDirectoryQueryFactory();

            Credentials = new DefaultCredentials();

            ActiveDirectoryProvider = new ActiveDirectoryPrincipalProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            Roles = ActiveDirectoryProvider.GetRoles("bcuser1").ToList(); //need to call ToList() as GetRoles is an iterator
        };

        public It should_not_have_called_GetMemberOfInfo = () =>
        {
            ActiveDirectoryProvider.GetMemberOfInfoWasCalled.ShouldBeFalse();
        };

        public It should_have_called_GetGroups = () =>
        {
            ActiveDirectoryProvider.GetGroupsWasCalled.ShouldBeTrue();
        };

        public It should_not_have_called_GetObjectSid = () =>
        {
            ActiveDirectoryProvider.GetObjectSidWasCalled.ShouldBeFalse();
        };

        public It should_have_retrieved_at_least_one_role = () =>
        {
            Roles.Any().ShouldBeTrue();
        };

        public It should_contain_the_domain_users_nested_group = () =>
        {
            Roles.Where(a => a == "Domain Users").Count().ShouldBeLike(1);
        };
    }
}