using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceCommandHandler<T> where T : IServiceCommand
    {
        ISystemMessageCollection HandleCommand(T command, IServiceRequestMetadata metadata);
    }
}