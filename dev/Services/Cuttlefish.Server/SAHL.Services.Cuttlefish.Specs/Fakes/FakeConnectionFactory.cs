using SAHL.Core.Logging;
using SAHL.Services.Cuttlefish.Managers;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeConnectionFactory : IQueueConnectionFactory
    {
        private IQueueConnection queueConnection;

        public FakeConnectionFactory(IQueueConnection queueConnection)
        {
            this.queueConnection = queueConnection;
        }

        public IQueueConnection InitialiseConnection(string queueServerName, string queueExchangeName, string queueName, string username, string password)
        {
            return queueConnection;
        }
    }
}