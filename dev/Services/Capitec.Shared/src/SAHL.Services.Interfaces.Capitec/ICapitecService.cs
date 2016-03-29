using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Capitec
{
    public interface ICapitecService
    {
        ISystemMessageCollection PerformCommand<T>(T Command, IServiceRequestMetadata metadata) where T : ICapitecServiceCommand;
    }
}