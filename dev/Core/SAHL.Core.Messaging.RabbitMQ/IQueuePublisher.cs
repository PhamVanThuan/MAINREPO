using RabbitMQ.Client;
using SAHL.Core.Messaging.Shared;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueuePublisher
    {
        void Publish<T>(string exchangeName, string routingKey, T message, bool durable, string exchangeType)
            where T : class, IMessage;
    }
}