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
    public class when_querying_for_memberOfInfo_with_security_identifiers_with_a_valid_user : WithFakes
    {
        private static ActiveDirectoryProviderExposed ActiveDirectoryProvider;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;
        private static List<IMemberOfInfo> MemberOfInfoResults;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = new ActiveDirectoryProviderCache();

            ActiveDirectoryQueryFactory = new ActiveDirectoryQueryFactory();

            Credentials = new DefaultCredentials();

            ActiveDirectoryProvider = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            MemberOfInfoResults = ActiveDirectoryProvider.GetMemberOfInfo("halouser", true).ToList(); //need to call ToList() as GetMemberOfInfo is an iterator
        };

        public It should_have_called_GetObjectSid = () =>
        {
            ActiveDirectoryProvider.GetObjectSidWasCalled.ShouldBeTrue();
        };

        public It should_have_retrieved_the_security_identifier = () =>
        {
            MemberOfInfoResults.Any(a => a.SecurityIdentifier == null).ShouldBeFalse();
        };

        public It should_have_retrieved_at_least_one_role = () =>
        {
            MemberOfInfoResults.Any().ShouldBeTrue();
        };

        public It should_have_cached_all_of_the_security_identifiers = () =>
        {
            ActiveDirectoryProviderCache.ObjectSidsByDistinguishedName.Count.ShouldEqual(MemberOfInfoResults.Count);
        };

        public It should_have_the_same_elements_in_cache_as_retrieved = () =>
        {
            ActiveDirectoryProviderCache.ObjectSidsByDistinguishedName
                .Select(a => new { a.Key, a.Value })
                .ShouldContainOnly(
                    MemberOfInfoResults.Select(a => new { Key = a.DistinguishedName, Value = a.SecurityIdentifier })
                );
        };
    }
}