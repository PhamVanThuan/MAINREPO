using SAHL.Core.Logging;
namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConnectionFactory
    {
        IQueueConnection InitialiseConnection(string queueServerName, string queueExchangeName, string queueName, string username, string password);
    }
}