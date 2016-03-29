using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.MessageHandlers;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Listens for asynchronous management messages (not intended for requests requiring a response).
    /// </summary>
    public interface IX2ManagementSubscriber : IX2ConsumerConfigurationProvider
    {

        void Subscribe(IX2RouteEndpoint route, IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest> messageHandler);
    }
}