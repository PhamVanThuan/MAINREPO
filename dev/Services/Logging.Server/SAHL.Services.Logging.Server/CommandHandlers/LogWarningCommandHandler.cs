using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Logging.Commands;

namespace SAHL.Services.Logging.CommandHandlers
{
    public class LogWarningCommandHandler : IServiceCommandHandler<LogWarningCommand>
    {
        ILogger logger;

        public LogWarningCommandHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(LogWarningCommand command, IServiceRequestMetadata metadata)
        {
            logger.LogWarning(new LoggerSource(command.Source, LogLevel.Warning, false), metadata.UserName, command.Message, command.Message);
            return SystemMessageCollection.Empty();
        }
    }
}
