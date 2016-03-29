using SAHL.Core.Logging;
using SAHL.Services.Cuttlefish.Services;
using System;

namespace SAHL.Services.Cuttlefish.Managers
{
    public class QueueConsumerFactory : IQueueConsumerFactory
    {
        private IMessageBusConfigurationProvider messageBusConfiguratrionProvider;
        private IQueueConnectionFactory queueInstanceFactory;
        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueConsumerFactory(IMessageBusConfigurationProvider messageBusConfiguratrionProvider, IQueueConnectionFactory queueInstanceFactory, ILogger logger, ILoggerSource loggerSource)
        {
            this.messageBusConfiguratrionProvider = messageBusConfiguratrionProvider;
            this.queueInstanceFactory = queueInstanceFactory;
            this.logger = logger;
            this.loggerSource = loggerSource;
        }

        public IQueueConsumer CreateQueueConsumer(string queueExchangeName, string queueName, Action<string> consumerAction, Func<bool> shouldCancel)
        {
            return new QueueConsumer(this.queueInstanceFactory, this.messageBusConfiguratrionProvider.HostName,
                queueExchangeName,
                queueName,
                this.messageBusConfiguratrionProvider.UserName,
                this.messageBusConfiguratrionProvider.Password, consumerAction, shouldCancel, this.logger, this.loggerSource);
        }
    }
}