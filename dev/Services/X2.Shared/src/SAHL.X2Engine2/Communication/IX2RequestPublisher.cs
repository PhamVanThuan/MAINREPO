using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Publishes a request message for the engine.
    /// </summary>
    public interface IX2RequestPublisher
    {
        void Publish<TRequest>(IX2RouteEndpoint routeEndpoint, TRequest request) where TRequest : class, SAHL.Core.Messaging.Shared.IMessage;
    }
}