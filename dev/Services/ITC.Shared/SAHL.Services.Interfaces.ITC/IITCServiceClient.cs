using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.ITC
{
    public interface IItcServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IITCServiceCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : IITCServiceQuery;
    }
}