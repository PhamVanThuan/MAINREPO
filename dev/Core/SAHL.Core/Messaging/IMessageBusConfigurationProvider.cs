using SAHL.Core.Configuration;

namespace SAHL.Core.Messaging
{
    public interface IMessageBusConfigurationProvider : IConfigurationProvider
    {
        string HostName { get; }

        string UserName { get; }

        string Password { get; }

        string SubscriptionId { get; }
    }
}