using SAHL.Core.Logging;
using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConsumerFactory : IQueueConsumerFactory
    {
        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueConsumerFactory(ILogger logger, ILoggerSource loggerSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
        }

        public IQueueConsumer CreateConsumer(IQueueConnection connection, string exchangeName, string queueName, Action<string> workAction, Func<bool> shouldCancel, string consumerId)
        {
            return new QueueConsumer(connection, exchangeName, queueName, workAction, shouldCancel, consumerId, logger, loggerSource);
        }
    }
}