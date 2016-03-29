using System;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeConsumerFactory : IQueueConsumerFactory
    {

        public FakeConsumerFactory(IQueueConsumer consumer)
        {
            this.Consumer = consumer;

        }

        public IQueueConsumer CreateConsumer(IQueueConnection connection, string exchangeName, string queueName, Action<string> workAction, Func<bool> shouldCancel, string consumerId)
        {
              return this.Consumer;
        }

        public IQueueConsumer Consumer { get; protected set; }
    }
}