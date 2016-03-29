using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Specs.Fakes;
using System.Linq;

namespace SAHL.Core.ActiveDirectory.Specs.ActiveDirectoryProviderIntegrationSpecs
{
    public class when_querying_for_memberOfInfo_with_a_valid_user_filter_checks : WithFakes
    {
        private static ActiveDirectoryProviderExposed ActiveDirectoryProvider;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = An<IActiveDirectoryProviderCache>();

            ActiveDirectoryQueryFactory = An<IActiveDirectoryQueryFactory>();

            Credentials = An<ICredentials>();

            ActiveDirectoryProvider = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            ActiveDirectoryProvider.GetMemberOfInfo("halouser").ToList(); //need to call ToList() as GetMemberOfInfo is an iterator
        };

        public It should_have_called_create_query_using_the_specified_credentials = () =>
        {
            ActiveDirectoryQueryFactory.WasToldTo(a => a.Create(Credentials, "(&(SAMAccountName=halouser))")).OnlyOnce();
        };
    }
}