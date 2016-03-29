using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private IQueueingBasicConsumer consumer;

        public RabbitMQConsumer(IQueueingBasicConsumer consumer)
        {
            this.consumer = consumer;
        }

        public bool Dequeue(int millisecondsTimeout, out BasicDeliverEventArgs result)
        {
            return this.consumer.Queue.Dequeue(millisecondsTimeout, out result);
        }
    }
}