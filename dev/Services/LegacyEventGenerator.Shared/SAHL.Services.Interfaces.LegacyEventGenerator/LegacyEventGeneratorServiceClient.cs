using SAHL.Core.Services;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.LegacyEventGenerator
{
    public class LegacyEventGeneratorServiceClient : ServiceHttpClient, ILegacyEventGeneratorServiceClient
    {
        public LegacyEventGeneratorServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public Core.SystemMessages.ISystemMessageCollection PerformCommand<T>(T command, Core.Services.IServiceRequestMetadata metadata) where T : ILegacyEventTriggerCommand
        {
            return base.PerformCommandInternal(command, metadata);
        }
    }
}