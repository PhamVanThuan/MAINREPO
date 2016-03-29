using RabbitMQ.Client;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private IConnectionFactory factory;

        public RabbitMQConnectionFactory(string hostName, string userName, string password, ushort heartbeatInterval)
        {
            try
            {
                factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password,
                    RequestedHeartbeat = heartbeatInterval
                };
            }
            catch
            {
                // this must not throw, just set the factory to null
                factory = null;
            }
        }

        public IConnection CreateConnection()
        {
            return this.factory.CreateConnection();
        }
    }
}