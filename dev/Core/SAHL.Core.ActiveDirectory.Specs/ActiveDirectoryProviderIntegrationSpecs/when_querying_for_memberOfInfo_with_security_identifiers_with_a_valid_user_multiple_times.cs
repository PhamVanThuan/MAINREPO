using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Query.Results;
using SAHL.Core.ActiveDirectory.Specs.Fakes;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.ActiveDirectory.Specs.ActiveDirectoryProviderIntegrationSpecs
{
    public class when_querying_for_memberOfInfo_with_security_identifiers_with_a_valid_user_multiple_times : WithFakes
    {
        private static ActiveDirectoryProviderExposed ActiveDirectoryProvider;
        private static ActiveDirectoryProviderExposed ActiveDirectoryProviderForSecondCall;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;
        private static List<IMemberOfInfo> MemberOfInfoResults;
        private static List<IMemberOfInfo> MemberOfInfoResultsForSecondCall;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = new ActiveDirectoryProviderCache();
            ActiveDirectoryQueryFactory = new ActiveDirectoryQueryFactory();
            Credentials = new DefaultCredentials();
            ActiveDirectoryProvider = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
            //using same cache to ensure we retrieve from cache, and not via query on second hit
            ActiveDirectoryProviderForSecondCall = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            const string username = "halouser";
            MemberOfInfoResults = ActiveDirectoryProvider.GetMemberOfInfo(username, true).ToList(); //need to call ToList() as GetMemberOfInfo is an iterator
            MemberOfInfoResultsForSecondCall = ActiveDirectoryProviderForSecondCall.GetMemberOfInfo(username, true).ToList();
        };

        public It should_have_the_same_elements_from_both_calls = () =>
        {
            MemberOfInfoResults
                .Select(a => new { a.CommonName, a.DistinguishedName, a.SecurityIdentifier })
                .ShouldContainOnly(
                    MemberOfInfoResultsForSecondCall.Select(a => new { a.CommonName, a.DistinguishedName, a.SecurityIdentifier })
                );
        };

        public It should_have_called_GetObjectSid_on_the_first_attempt = () =>
        {
            ActiveDirectoryProvider.GetObjectSidWasCalled.ShouldBeTrue();
        };

        public It should_have_called_GetObjectSidViaQuery_on_the_first_attempt = () =>
        {
            ActiveDirectoryProvider.GetObjectSidViaQueryWasCalled.ShouldBeTrue();
        };

        public It should_not_have_called_GetObjectSid_on_the_second_attempt = () =>
        {
            ActiveDirectoryProviderForSecondCall.GetObjectSidViaQueryWasCalled.ShouldBeFalse();
        };
    }
}