using SAHL.Core.Messaging.Shared;
using System;

namespace SAHL.Core.Messaging
{
    public interface IMessageBusAdvanced : IMessageBus
    {
        void Publish(IMessage message);

        void Publish<T>(IMessageRoute route, T message) where T : class, IMessage;

        void Subscribe<T>(IMessageRoute route, Action<T> subscriptionHandler) where T : class, IMessage;

        void Subscribe<T>(string subscriptionId, Action<T> subscriptionHandler) where T : class, IMessage;

        void DeclareQueue(string exchangeName, string queueName, bool isDurable);

        void DeclareExchange(string exchangeName);
    }
}