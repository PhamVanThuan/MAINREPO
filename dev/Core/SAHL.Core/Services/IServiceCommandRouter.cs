using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceCommandRouter
    {
        ISystemMessageCollection HandleCommand<T>(T command, IServiceRequestMetadata metaData) where T : IServiceCommand;

        ISystemMessageCollection HandleCommand(object command, IServiceRequestMetadata metaData);
    }
}