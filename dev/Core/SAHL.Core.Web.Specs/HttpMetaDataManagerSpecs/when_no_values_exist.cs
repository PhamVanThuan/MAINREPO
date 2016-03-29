using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using System.Security.Principal;

namespace SAHL.Core.Web.Specs.MetaDataManagerSpecs
{
    [Subject("SAHL.Core.Web.Services.MetaDataManager.GetMetaData")]
    public class when_no_values_exist : WithFakes
    {
        private static IHostContext hostContext;
        private static IMetadataManager metaDataManager;
        private static IServiceRequestMetadata result;

        private Establish context = () =>
        {
            string[] keys = new string[] { };
            hostContext = An<IHostContext>();
            hostContext.WhenToldTo(x => x.GetKeysWithPrefix(ServiceHttpClient.MetaHeaderPrefix))
                .Return(keys);

            hostContext.WhenToldTo(x => x.GetContextValue(Param<string>.IsAnything, ServiceHttpClient.MetaHeaderPrefix)).Return(string.Empty);
            hostContext.WhenToldTo(x => x.GetUser()).Return(default(IPrincipal));

            metaDataManager = new MetadataManager(hostContext);
        };

        private Because of = () =>
        {
            result = metaDataManager.GetMetaData();
        };

        private It should_not_try_and_get_username_from_header = () =>
        {
            hostContext.WasNotToldTo(x => x.GetContextValue(MetadataManager.HEADER_USERNAME, ServiceHttpClient.MetaHeaderPrefix));
        };

        private It should_try_get_username_from_context = () =>
        {
            hostContext.WasToldTo(x => x.GetUser());
        };

        private It should_contain_user_name_from_context = () =>
        {
            result.ShouldContain(x => x.Key == MetadataManager.HEADER_USERNAME);
        };
    }
}