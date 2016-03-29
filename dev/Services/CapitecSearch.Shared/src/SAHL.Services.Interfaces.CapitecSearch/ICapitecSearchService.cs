using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.CapitecSearch
{
    public interface ICapitecSearchService
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ICapitecSearchServiceCommand;
    }
}