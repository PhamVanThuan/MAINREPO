using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.ITC
{
    public class ItcServiceClient : ServiceHttpClientWindowsAuthenticated, IItcServiceClient
    {
        public ItcServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IITCServiceCommand
        {
            return this.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IITCServiceQuery
        {
            return this.PerformQueryInternal<T>(query);
        }
    }
}