using System;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication
{
    /// <summary>
    /// Waits on x2 response for the engine.
    /// </summary>
    public interface IX2ResponseSubscriber : IX2ConsumerConfigurationProvider
    {
        void Subscribe<TResponse>(Action<TResponse> responseCallback) where TResponse : X2Response;
    }
}