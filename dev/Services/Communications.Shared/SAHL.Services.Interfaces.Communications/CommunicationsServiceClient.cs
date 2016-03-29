using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Communications;

namespace SAHL.Services.Interfaces.Communications
{
    public class CommunicationsServiceClient : ServiceHttpClientWindowsAuthenticated, ICommunicationsServiceClient
    {
        public CommunicationsServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata serviceRequestMetaData) where T : ICommunicationsServiceCommand
        {
            return base.PerformCommandInternal<T>(command, serviceRequestMetaData);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : ICommunicationsQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}