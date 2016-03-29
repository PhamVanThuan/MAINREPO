using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.LegacyEventGenerator
{
    public interface ILegacyEventGeneratorServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ILegacyEventTriggerCommand;
    }
}