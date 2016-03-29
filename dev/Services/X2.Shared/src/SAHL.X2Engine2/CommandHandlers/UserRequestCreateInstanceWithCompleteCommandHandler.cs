using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Factories;
using SAHL.X2Engine2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UserRequestCreateInstanceWithCompleteCommandHandler : IServiceCommandHandler<UserRequestCreateInstanceWithCompleteCommand>
    {
        private IMessageCollectionFactory messageCollectionFactory;
        private IX2ServiceCommandRouter commandHandler;

        public UserRequestCreateInstanceWithCompleteCommandHandler(IMessageCollectionFactory messageCollectionFactory, IX2ServiceCommandRouter commandHandler)
        {
            this.messageCollectionFactory = messageCollectionFactory;
            this.commandHandler = commandHandler;
        }

        public ISystemMessageCollection HandleCommand(UserRequestCreateInstanceWithCompleteCommand command, IServiceRequestMetadata metadata)
        {
            var messages = messageCollectionFactory.CreateEmptyCollection();
            
            // create instance
            UserRequestCreateInstanceCommand userRequestCreateInstanceCommand = new UserRequestCreateInstanceCommand(command.ProcessName, command.WorkflowName, command.Username, command.Activity, command.MapVariables, command.WorkflowProviderName, command.IgnoreWarnings);
            messages.Aggregate(this.commandHandler.HandleCommand(userRequestCreateInstanceCommand, metadata));
            command.NewlyCreatedInstanceId = userRequestCreateInstanceCommand.NewlyCreatedInstanceId;

            // create complete
            if (!messages.HasErrors)
            {
                UserRequestCompleteCreateCommand userRequestCompleteCreateCommand = new UserRequestCompleteCreateCommand(command.NewlyCreatedInstanceId, command.Activity, command.Username, command.IgnoreWarnings, command.MapVariables, null);
                messages.Aggregate(this.commandHandler.HandleCommand(userRequestCompleteCreateCommand, metadata));
            }

            return messages;
        }
    }
}
