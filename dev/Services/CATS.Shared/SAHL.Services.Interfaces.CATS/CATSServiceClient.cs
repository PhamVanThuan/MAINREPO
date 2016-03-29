using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.CATS
{
    public class CATSServiceClient : ServiceHttpClientWindowsAuthenticated, ICATSServiceClient
    {
        public CATSServiceClient(IServiceUrlConfigurationProvider serviceConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, Core.Services.IServiceRequestMetadata serviceRequestMetadata) where T : ICATSServiceCommand
        {
            return base.PerformCommandInternal<T>(command, serviceRequestMetadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : ICATSServiceQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}