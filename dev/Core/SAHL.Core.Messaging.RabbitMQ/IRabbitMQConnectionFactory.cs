using RabbitMQ.Client;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();
    }
}