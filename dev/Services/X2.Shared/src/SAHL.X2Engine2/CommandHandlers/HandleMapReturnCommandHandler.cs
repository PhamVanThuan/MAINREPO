using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class HandleMapReturnCommandHandler : IServiceCommandHandler<HandleMapReturnCommand>, IDontDecorateServiceCommand
    {
        IX2ServiceCommandRouter commandHandler;

        public HandleMapReturnCommandHandler(IX2ServiceCommandRouter commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public ISystemMessageCollection HandleCommand(HandleMapReturnCommand command, IServiceRequestMetadata metadata)
        {
            if (command.Result == false)
            {
                throw new MapReturnedFalseException(command.Messages);
            }
            return command.Messages;
        }
    }
}