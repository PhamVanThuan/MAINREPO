using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Query.Results;
using SAHL.Core.ActiveDirectory.Specs.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.ActiveDirectory.Specs.ActiveDirectoryProviderIntegrationSpecs
{
    public class when_querying_for_memberOfInfo_with_a_valid_user : WithFakes
    {
        private static ActiveDirectoryProviderExposed ActiveDirectoryProvider;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;
        private static List<IMemberOfInfo> MemberOfInfoResults;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = An<IActiveDirectoryProviderCache>();

            Credentials = An<ICredentials>();

            ActiveDirectoryQueryFactory = new ActiveDirectoryQueryFactory();

            ActiveDirectoryProvider = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            MemberOfInfoResults = ActiveDirectoryProvider.GetMemberOfInfo("halouser").ToList(); //need to call ToList() as GetMemberOfInfo is an iterator
        };

        public It should_have_called_GetMemberOfInfo = () =>
        {
            ActiveDirectoryProvider.GetMemberOfInfoWasCalled.ShouldBeTrue();
        };

        public It should_not_have_called_GetObjectSid = () =>
        {
            ActiveDirectoryProvider.GetObjectSidWasCalled.ShouldBeFalse();
        };

        public It should_have_not_retrieved_the_security_identifier = () =>
        {
            MemberOfInfoResults.All(a => a.SecurityIdentifier == null).ShouldBeTrue();
        };

        public It should_have_retrieved_at_least_one_role = () =>
        {
            MemberOfInfoResults.Any().ShouldBeTrue();
        };
    }
}