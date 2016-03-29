using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public interface IX2ServiceCommandRouter
    {
        bool CheckRuleCommand<T>(T command, IServiceRequestMetadata metadata) where T : IRuleCommand;

        void QueueUpCommandToBeProcessed<T>(T command) where T : IServiceCommand;

        void ProcessQueuedCommands(IServiceRequestMetadata metadata);

        ISystemMessageCollection HandleCommand<T>(T command, IServiceRequestMetadata metadata) where T : IServiceCommand;
    }
}