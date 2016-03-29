using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UserRequestCancelActivityCommandHandler : IServiceCommandHandler<UserRequestCancelActivityCommand>
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IX2ServiceCommandRouter commandHandler;

        public UserRequestCancelActivityCommandHandler(IX2ServiceCommandRouter commandHandler, IWorkflowDataProvider workflowDataProvider)
        {
            this.commandHandler = commandHandler;
            this.workflowDataProvider = workflowDataProvider;
        }

        public ISystemMessageCollection HandleCommand(UserRequestCancelActivityCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            var instance = workflowDataProvider.GetInstanceDataModel(command.InstanceId);

            var checkActivityIsValidForStateCommand = new CheckActivityIsValidForStateCommand(instance, command.Activity.StateId);
            commandHandler.CheckRuleCommand(checkActivityIsValidForStateCommand, metadata);

            if (instance.StateID == Constants.NewlyCreatedInstanceStateID)
            {
                var deleteInstanceCommand = new DeleteInstanceCommand(command.InstanceId);
                messages.Aggregate(commandHandler.HandleCommand(deleteInstanceCommand, metadata));
            }
            else
            {
                var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
                messages.Aggregate(commandHandler.HandleCommand(unlockInstanceCommand, metadata));
            }
            return messages;
        }
    }
}