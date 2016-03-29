namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IRabbitMQConnectionFactoryFactory
    {
        IRabbitMQConnectionFactory CreateFactory();
    }
}