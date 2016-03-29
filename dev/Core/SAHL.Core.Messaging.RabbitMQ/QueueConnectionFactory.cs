using SAHL.Core.Logging;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConnectionFactory : IQueueConnectionFactory
    {
        private ILogger logger;
        private ILoggerSource loggerSource;
        private IRabbitMQConnectionFactoryFactory rabbitMQConnectionFactoryFactory;
        private QueueConnection connection;

        public QueueConnectionFactory(ILogger logger, ILoggerSource loggerSource, IRabbitMQConnectionFactoryFactory rabbitMQConnectionFactoryFactory)
        {
            this.rabbitMQConnectionFactoryFactory = rabbitMQConnectionFactoryFactory;
            this.logger = logger;
            this.loggerSource = loggerSource;
        }

        public IQueueConnection CreateConnection()
        {
            // this should always be a singleton
            if (this.connection == null)
            {
                this.connection = new QueueConnection(logger, loggerSource, rabbitMQConnectionFactoryFactory);
            }
            return this.connection;
        }
    }
}