using RabbitMQ.Client;
using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IRabbitMQModel : IDisposable
    {
        void ExchangeDeclare(string exchangeName, string type, bool durable);

        void QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete);

        void QueueBind(string queueName, string exchangeName, string routingKey);

        void SetQos(uint prefetchSize, ushort prefetchCount, bool global);

        IRabbitMQConsumer SetupConsumerForQueue(string queueName, bool noAck);

        void BasicAck(ulong deliveryTag, bool multiple);

        void BasicNack(ulong deliveryTag, bool multiple, bool requeue);

        void BasicPublish(string exchangeName, string routingKey, IBasicProperties properties, byte[] messageBodyBytes);

        IBasicProperties CreateBasicProperties();
    }
}