using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckCreationActivityAccessCommandHandler : IServiceCommandHandler<CheckCreationActivityAccessCommand>
    {
        public ISystemMessageCollection HandleCommand(CheckCreationActivityAccessCommand command, IServiceRequestMetadata metadata)
        {
            // If the current State is null its in the process of a create.
            if (command.Activity.StateId == null)
            {
                command.Result = true;
                return new SystemMessageCollection(); ;
            }
            command.Result = false;
            return new SystemMessageCollection();
        }
    }
}