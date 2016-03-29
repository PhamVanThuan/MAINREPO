using SAHL.Core.Logging;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueueConnectionFactory
    {
        IQueueConnection CreateConnection();
    }
}