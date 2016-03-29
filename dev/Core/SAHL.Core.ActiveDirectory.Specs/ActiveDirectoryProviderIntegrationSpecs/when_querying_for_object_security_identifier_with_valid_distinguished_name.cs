using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.ActiveDirectory.Credentials;
using SAHL.Core.ActiveDirectory.Provider;
using SAHL.Core.ActiveDirectory.Query;
using SAHL.Core.ActiveDirectory.Specs.Fakes;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Specs.ActiveDirectoryProviderIntegrationSpecs
{
    public class when_querying_for_object_security_identifier_with_valid_distinguished_name : WithFakes
    {
        private static ActiveDirectoryProviderExposed ActiveDirectoryProvider;
        private static IActiveDirectoryQueryFactory ActiveDirectoryQueryFactory;
        private static IActiveDirectoryProviderCache ActiveDirectoryProviderCache;
        private static ICredentials Credentials;
        private static SecurityIdentifier SecurityIdentifier;

        Establish context = () =>
        {
            ActiveDirectoryProviderCache = new ActiveDirectoryProviderCache();

            ActiveDirectoryQueryFactory = new ActiveDirectoryQueryFactory();

            Credentials = new DefaultCredentials();

            ActiveDirectoryProvider = new ActiveDirectoryProviderExposed(ActiveDirectoryQueryFactory, ActiveDirectoryProviderCache, Credentials);
        };

        public Because of = () =>
        {
            SecurityIdentifier = ActiveDirectoryProvider.GetObjectSid("CN=IT Developers,OU=Groups &  Service acc's,OU=IT,OU=Head Office,DC=SAHL,DC=com");
        };

        public It should_have_called_GetObjectSid = () =>
        {
            ActiveDirectoryProvider.GetObjectSidWasCalled.ShouldBeTrue();
        };

        public It should_match_the_existing_security_identifier = () =>
        {
            SecurityIdentifier.ShouldEqual(new SecurityIdentifier("S-1-5-21-420440691-55313521-1349916565-10502"));
        };
    }
}