using SAHL.Core.Messaging.Shared;
using System;

namespace SAHL.Core.Messaging
{
    public interface IMessageBus : IDisposable
    {
        void Publish<T>(T message)
            where T : class, IMessage;

        void Subscribe<T>(Action<T> subscriptionHandler)
            where T : class, IMessage;
    }
}