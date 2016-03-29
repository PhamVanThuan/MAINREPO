namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeConnectionFactory : IQueueConnectionFactory
    {
        private IQueueConnection connection;

        public FakeConnectionFactory(IQueueConnection connection)
        {
            this.connection = connection;
        }

        public IQueueConnection CreateConnection()
        {
            return this.connection;
        }
    }
}