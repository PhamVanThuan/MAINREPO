using SAHL.Core;
using SAHL.Core.Messaging.RabbitMQ;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class RabbitMQConsumerManager : IX2ConsumerManager
    {
        private IIocContainer container;
        private IQueueConsumerManager queueConsumerManager;

        public RabbitMQConsumerManager(IIocContainer container, IQueueConsumerManager queueConsumerManager)
        {
            this.container = container;
            this.queueConsumerManager = queueConsumerManager;
        }

        public void Initialise()
        {
            var consumerConfigurationProviders = container.GetAllInstances<IX2ConsumerConfigurationProvider>();
            List<QueueConsumerConfiguration> queueConsumers = new List<QueueConsumerConfiguration>();
            foreach (var provider in consumerConfigurationProviders)
            {
                queueConsumers.AddRange(provider.GetConsumers());
            }

#if DEBUG
                System.Console.WriteLine("Starting consumers");
#endif
            this.queueConsumerManager.StartConsumers(queueConsumers);
        }

        public void TearDown()
        {
            this.queueConsumerManager.StopAllConsumers();
        }
    }
}