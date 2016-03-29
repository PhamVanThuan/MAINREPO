using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Publishes a response message for the node.
    /// </summary>
    public interface IX2ResponsePublisher
    {
        void Publish<T>(IX2RouteEndpoint route, T response) where T : class, IX2Message;
    }
}