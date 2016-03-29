using SAHL.Core.Logging;
namespace SAHL.Services.Cuttlefish.Managers
{
    public class QueueInstanceFactory : IQueueConnectionFactory
    {
        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueInstanceFactory(ILogger logger, ILoggerSource loggerSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
        }

        public IQueueConnection InitialiseConnection(string queueServerName, string queueExchangeName, string queueName, string username, string password)
        {
            return new QueueConnection(queueServerName, queueExchangeName, queueName, username, password, this.logger, this.loggerSource);
        }
    }
}