using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using System.Security.Principal;

namespace SAHL.Core.Web.Specs.MetaDataManagerSpecs
{
    [Subject("SAHL.Core.Web.Services.MetaDataManager.GetMetaData")]
    public class when_ipaddress_exists_in_header : WithFakes
    {
        private static IPrincipal principalUser;
        private static IHostContext hostContext;
        private static IMetadataManager metaDataManager;
        private static IServiceRequestMetadata result;

        private static string expectedUserName = "TestIPrincpleName";
        private static string expectedIPAddress = "127::0::0::1";//et go home

        private Establish context = () =>
        {
            string[] keys = new string[] { MetadataManager.HEADER_USERNAME };
            hostContext = An<IHostContext>();
            hostContext.WhenToldTo(x => x.GetKeysWithPrefix(ServiceHttpClient.MetaHeaderPrefix))
                .Return(keys);

            principalUser = An<IPrincipal>();
            IIdentity identity = An<IIdentity>();
            identity.WhenToldTo(x => x.Name).Return(expectedUserName);
            principalUser.WhenToldTo(x => x.Identity).Return(identity);

            hostContext.WhenToldTo(x => x.GetContextValue(MetadataManager.HEADER_USERNAME, ServiceHttpClient.MetaHeaderPrefix)).Return(string.Empty);
            hostContext.WhenToldTo(x => x.GetContextValue(MetadataManager.HEADER_USERIPADDRESS, ServiceHttpClient.MetaHeaderPrefix)).Return(expectedIPAddress);
            hostContext.WhenToldTo(x => x.GetUser()).Return(principalUser);

            metaDataManager = new MetadataManager(hostContext);
        };

        private Because of = () =>
        {
            result = metaDataManager.GetMetaData();
        };

        private It should_try_and_get_username_from_header = () =>
        {
            hostContext.WasToldTo(x => x.GetContextValue(MetadataManager.HEADER_USERNAME, ServiceHttpClient.MetaHeaderPrefix));
        };

        private It should_not_get_username_from_context = () =>
        {
            hostContext.WasToldTo(x => x.GetUser());
        };

        private It should_get_user_name = () =>
        {
            result[MetadataManager.HEADER_USERNAME].ShouldEqual(expectedUserName);
        };
    }
}