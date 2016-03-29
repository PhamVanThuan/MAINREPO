using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Logging.Commands;

namespace SAHL.Services.Logging.CommandHandlers
{
    public class LogErrorCommandHandler : IServiceCommandHandler<LogErrorCommand>
    {
        ILogger logger;

        public LogErrorCommandHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(LogErrorCommand command, IServiceRequestMetadata metadata)
        {
            logger.LogFormattedError(new LoggerSource(command.Source, LogLevel.Error, false), metadata.UserName, command.Message, command.StackTrace);
            return SystemMessageCollection.Empty();
        }
    }
}
