using SAHL.Core.Configuration;
using System;

namespace SAHL.Core.Logging.Loggers.MessageBus
{
    public class MessageBusLoggerConfigurationProvider : ConfigurationProvider, IMessageBusLoggerConfigurationProvider
    {
        public LogLevel LogLevel
        {
            get { return (LogLevel)Enum.Parse(typeof(LogLevel), this.Config.AppSettings.Settings["LogLevel"].Value); }
        }
    }
}