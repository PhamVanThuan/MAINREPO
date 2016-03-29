using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.DocumentManager
{
    public class DocumentManagerServiceClient : ServiceHttpClientWindowsAuthenticated, IDocumentManagerServiceClient
    {
        public DocumentManagerServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IDocumentManagerCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }


        public ISystemMessageCollection PerformQuery<T>(T query) where T : IDocumentManagerQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}