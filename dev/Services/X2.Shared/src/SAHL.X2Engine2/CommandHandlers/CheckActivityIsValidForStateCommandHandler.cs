using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.Core.X2.Exceptions;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckActivityIsValidForStateCommandHandler : IServiceCommandHandler<CheckActivityIsValidForStateCommand>
    {
        public ISystemMessageCollection HandleCommand(CheckActivityIsValidForStateCommand command, IServiceRequestMetadata metadata)
        {
            // If the current State is null its in the process of a create. The fact that we managed to get the Activity propery of the command loada
            // means we are all good
            string errorMessage = string.Empty;
            if (command.Instance.StateID == null)
            {
                command.Result = true;
                return new SystemMessageCollection();
            }
            // if the state the instance is a valid "from" state then its valid
            if (command.Instance.StateID == command.StateId)
            {
                command.Result = true;
                return new SystemMessageCollection();
            }
            // return workflowactivity doesnt have a
            if (command.StateId == null)
            {
                command.Result = true;
                return new SystemMessageCollection();
            }
            errorMessage = string.Format("Activity is not valid for the state of this instance {2}. Instance is at {0} should be {1}", command.Instance.StateID, command.StateId, command.Instance.ID);
            throw new RuleCommandException(errorMessage);
        }
    }
}