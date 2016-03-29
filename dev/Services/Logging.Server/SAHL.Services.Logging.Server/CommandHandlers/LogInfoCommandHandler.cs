using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Logging.Commands;

namespace SAHL.Services.Logging.CommandHandlers
{
    public class LogInfoCommandHandler : IServiceCommandHandler<LogInfoCommand>
    {
        ILogger logger;

        public LogInfoCommandHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(LogInfoCommand command, IServiceRequestMetadata metadata)
        {
            logger.LogInfo(new LoggerSource(command.Source, LogLevel.Info, false), metadata.UserName, command.Message, command.Message);
            return SystemMessageCollection.Empty();
        }
    }
}
