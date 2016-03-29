using System.Collections.Generic;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using StructureMap;
using System.Linq;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class X2ServiceCommandRouter : IX2ServiceCommandRouter
    {
        private IServiceCommandHandlerProvider serviceCommandHandlerProvider;
        private IX2ServiceCommandRouter commandHandler;

        [DefaultConstructor]
        public X2ServiceCommandRouter(IServiceCommandHandlerProvider serviceCommandHandlerProvider)
        {
            this.serviceCommandHandlerProvider = serviceCommandHandlerProvider;
            this.commandHandler = this;
        }

        public X2ServiceCommandRouter(IServiceCommandHandlerProvider serviceCommandHandlerProvider, IX2ServiceCommandRouter commandHandler)
        {
            this.serviceCommandHandlerProvider = serviceCommandHandlerProvider;
            this.commandHandler = commandHandler;
        }

        public ISystemMessageCollection HandleCommand<T>(T command, IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            var commandHandlerToRun = serviceCommandHandlerProvider.GetCommandHandler<T>();
            return commandHandlerToRun.HandleCommand(command, metadata);
        }

        public bool CheckRuleCommand<T>(T command, IServiceRequestMetadata metadata) where T : IRuleCommand
        {
            var commandHandlerToRun = serviceCommandHandlerProvider.GetCommandHandler<T>();
            commandHandlerToRun.HandleCommand(command, metadata);
            return command.Result;
        }

        private List<IServiceCommand> commands = new List<IServiceCommand>();

        public void QueueUpCommandToBeProcessed<T>(T command) where T : IServiceCommand
        {
            commands.Add(command);
        }

        public void ProcessQueuedCommands(IServiceRequestMetadata metadata)
        {
            if (commands.Count > 0)
            {
                List<IServiceCommand> bundledRequests = new List<IServiceCommand>();
                foreach (var command in commands)
                {
                    if (command is PublishBundledRequestCommand)
                    {
                        bundledRequests.Add(command);
                    }
                    else
                    {
                        commandHandler.HandleCommand((dynamic)command, metadata);
                    }
                }

                foreach (var bundledRequest in bundledRequests)
                {
                    commandHandler.HandleCommand((dynamic)bundledRequest, metadata);
                }
                bundledRequests.Clear();
            }
            
            commands.Clear();
        }
    }
}