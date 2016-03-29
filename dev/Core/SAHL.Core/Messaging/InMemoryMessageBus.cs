using SAHL.Core.Messaging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Messaging
{
    public class InMemoryMessageBus : IMessageBusAdvanced
    {
        private readonly IMessageRouteNameBuilder routeNameBuilder;
        private Dictionary<Type, List<MessageHandler>> handlerLookup;

        public InMemoryMessageBus(IMessageRouteNameBuilder routeNameBuilder)
        {
            this.routeNameBuilder = routeNameBuilder;
            handlerLookup = new Dictionary<Type, List<MessageHandler>>();
        }

        public InMemoryMessageBus()
            : this(new DefaultRouteNameBuilder<MessageRoute>())
        {
        }

        public void Publish(IMessage message)
        {
            DispatchByType(message);
        }

        public void Publish<T>(T message) where T : class, IMessage
        {
            DispatchByType(message);
        }

        public void Subscribe<T>(Action<T> subscriptionHandler) where T : class, IMessage
        {
            AddHandlerToLookup(subscriptionHandler);
        }

        public void Dispose()
        {
            handlerLookup.Clear();
            handlerLookup = null;
        }

        public void Publish<T>(IMessageRoute route, T message) where T : class, IMessage
        {
            var topic = routeNameBuilder.BuildRouteName(route);
            DispatchByType(message, topic);
        }

        public void Subscribe<T>(IMessageRoute route, Action<T> subscriptionHandler) where T : class, IMessage
        {
            var topic = routeNameBuilder.BuildRouteName(route);
            AddHandlerToLookup(subscriptionHandler, topic);
        }

        public void Subscribe<T>(string subscriptionId, Action<T> subscriptionHandler) where T : class, IMessage
        {
            AddHandlerToLookup(subscriptionHandler);
        }

        private void AddHandlerToLookup<TMessage>(Action<TMessage> handler, string topic = null)
        {
            List<MessageHandler> handlers;
            var type = typeof(TMessage);
            if (!handlerLookup.TryGetValue(type, out handlers))
            {
                handlers = new List<MessageHandler>();
                handlerLookup.Add(type, handlers);
            }
            if (!handlers.Any(x => x.Equals(handler)))
            {
                Action<IMessage> actionHandler = message =>
                {
                    handler((TMessage)message);
                };

                handlers.Add(new MessageHandler(actionHandler, topic));
            }
        }

        private void DispatchByType(IMessage message, string topic = null)
        {
            var type = message.GetType();
            do
            {
                DispatchByType(message, topic, type);
                type = type.BaseType;
            } while (type != null && type != message.GetType());
        }

        private void DispatchByType(IMessage message, string topic, Type type)
        {
            List<MessageHandler> list;
            if (!handlerLookup.TryGetValue(type, out list))
            {
                return;
            }
            foreach (var handler in list.Where(x => x.Topic == null || x.Topic == topic))
            {
                handler.HandleCommand(message);
            }
        }

        public bool IsSerializable(object objectToSerialize)
        {
            try
            {
                Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void DeclareQueue(string queueName, bool isDurable)
        {
            throw new NotImplementedException();
        }


        public void DeclareQueue(string exchangeName, string queueName, bool isDurable)
        {
            throw new NotImplementedException();
        }

        public void DeclareExchange(string exchangeName)
        {
            throw new NotImplementedException();
        }
    }
}