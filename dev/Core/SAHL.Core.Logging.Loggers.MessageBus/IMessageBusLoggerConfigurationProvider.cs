using SAHL.Core.Configuration;

namespace SAHL.Core.Logging.Loggers.MessageBus
{
    public interface IMessageBusLoggerConfigurationProvider : IConfigurationProvider
    {
        LogLevel LogLevel { get; }
    }
}