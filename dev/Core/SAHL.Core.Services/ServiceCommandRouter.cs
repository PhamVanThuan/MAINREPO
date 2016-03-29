using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public class ServiceCommandRouter : IServiceCommandRouter
    {
        private IServiceCommandHandlerProvider commandHandlerProvider;

        public ServiceCommandRouter(IServiceCommandHandlerProvider commandHandlerProvider)
        {
            this.commandHandlerProvider = commandHandlerProvider;
        }

        public ISystemMessageCollection HandleCommand<T>(T command, IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            IServiceCommandHandler<T> handler = this.commandHandlerProvider.GetCommandHandler<T>();
            return handler.HandleCommand(command, metadata);
        }

        public ISystemMessageCollection HandleCommand(object command, IServiceRequestMetadata metadata)
        {
            dynamic handler = this.commandHandlerProvider.GetCommandHandler(command);
            return handler.HandleCommand((dynamic)command, metadata);
        }
    }
}