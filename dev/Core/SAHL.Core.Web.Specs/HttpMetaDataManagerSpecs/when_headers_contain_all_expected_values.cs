using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using System.Security.Principal;

namespace SAHL.Core.Web.Specs.MetaDataManagerSpecs
{
    [Subject("SAHL.Core.Web.Services.MetaDataManager.GetMetaData")]
    public class when_headers_contain_all_expected_values : WithFakes
    {
        private static IHostContext hostContext;
        private static IMetadataManager metaDataManager;
        private static IServiceRequestMetadata result;

        private static string expectedUserName = "TestIPrincpleName";

        private Establish context = () =>
        {
            string[] keys = new string[] { MetadataManager.HEADER_USERNAME };
            hostContext = An<IHostContext>();
            hostContext.WhenToldTo(x => x.GetKeysWithPrefix(ServiceHttpClient.MetaHeaderPrefix))
                .Return(keys);

            hostContext.WhenToldTo(x => x.GetContextValue(MetadataManager.HEADER_USERNAME, ServiceHttpClient.MetaHeaderPrefix)).Return(expectedUserName);

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
            hostContext.WasNotToldTo(x => x.GetUser());
        };

        private It should_get_user_name = () =>
        {
            result[MetadataManager.HEADER_USERNAME].ShouldEqual(expectedUserName);
        };
    }
}
