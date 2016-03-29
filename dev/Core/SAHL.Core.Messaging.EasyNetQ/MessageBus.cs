using EasyNetQ;
using EasyNetQ.Topology;
using SAHL.Core.Messaging.Shared;
using SAHL.Core.Strings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public class MessageBus : IMessageBusAdvanced
    {
        private readonly IMessageBusConfigurationProvider messageBusConfigurationProvider;
        private readonly Dictionary<Type, IMessageRouteNameBuilder> routeNameBuilders;
        private IBus messageBus;

        public MessageBus(IMessageBusConfigurationProvider messageBusConfigurationProvider, IMessageRouteNameBuilder[] routeNameBuilders, IEasyNetQMessageBusSettings settings)
        {
            this.messageBusConfigurationProvider = messageBusConfigurationProvider;
            this.routeNameBuilders = new Dictionary<Type, IMessageRouteNameBuilder>();

            foreach (var builder in routeNameBuilders)
            {
                Type builderType = builder.GetType();
                Type genericTypeDefinition = builderType.GetInterfaces().SingleOrDefault(x => x.Name == typeof(IMessageRouteNameBuilder<>).Name);
                if (genericTypeDefinition == null)
                {
                    continue;
                }
                Type routeType = genericTypeDefinition.GetGenericArguments().FirstOrDefault();
                if (routeType != null)
                {
                    this.routeNameBuilders.Add(routeType, builder);
                }
            }

            Initialise(settings);
        }

        private void Initialise(IEasyNetQMessageBusSettings settings)
        {
            if (messageBus != null)
            {
                return;
            }

            var connectionString = String.Format("host={0}", messageBusConfigurationProvider.HostName);
            if (!String.IsNullOrEmpty(messageBusConfigurationProvider.UserName))
            {
                connectionString = String.Format("host={0};username={1};password={2};requestedHeartbeat=0", messageBusConfigurationProvider.HostName, 
                                                                                        messageBusConfigurationProvider.UserName, 
                                                                                        messageBusConfigurationProvider.Password);
            }
            messageBus = RabbitHutch.CreateBus(connectionString, x =>
            {
                if (settings != null)
                {
                    settings.RegisterServices(x);
                }
            });
        }

        public void Publish(SAHL.Core.Messaging.Shared.IMessage message)
        {
            dynamic internalMessage = message;
            messageBus.Publish(internalMessage);
        }

        public void Publish<T>(T message)
            where T : class, SAHL.Core.Messaging.Shared.IMessage
        {
            messageBus.Publish(message);
        }

        public void Subscribe<T>(Action<T> subscriptionHandler)
            where T : class, SAHL.Core.Messaging.Shared.IMessage
        {
            messageBus.Subscribe(messageBusConfigurationProvider.SubscriptionId, subscriptionHandler);
        }

        public virtual void Dispose()
        {
            messageBus.Dispose();
            messageBus = null;
        }

        public void Publish<T>(IMessageRoute route, T message)
            where T : class, SAHL.Core.Messaging.Shared.IMessage
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            var exchange = messageBus.Advanced.ExchangeDeclare(route.ExchangeName, ExchangeType.Direct);
            messageBus.Advanced.Publish(exchange, "#", true, true, new Message<T>(message));
        }

        public void Subscribe<T>(IMessageRoute route, Action<T> subscriptionHandler) where T : class, SAHL.Core.Messaging.Shared.IMessage
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            var routeNameBuilder = GetRouteNameBuilderForRoute(route);
            var topic = routeNameBuilder.BuildRouteName(route);

            messageBus.Subscribe(messageBusConfigurationProvider.SubscriptionId, subscriptionHandler, x => x.WithTopic(topic));
        }

        public void Subscribe<T>(string subscriptionId, Action<T> subscriptionHandler) where T : class, SAHL.Core.Messaging.Shared.IMessage
        {
            var adjustedSubscriptionId = subscriptionId.Length > 200 ? subscriptionId.Shorten() : subscriptionId;
            messageBus.Subscribe(adjustedSubscriptionId, subscriptionHandler);
        }

        private IMessageRouteNameBuilder GetRouteNameBuilderForRoute(IMessageRoute route)
        {
            Type routeType = route.GetType();
            return routeNameBuilders[routeType];
        }

        public void DeclareQueue(string exchangeName, string queueName, bool isDurable)
        {
            var exchange = messageBus.Advanced.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            var queue = messageBus.Advanced.QueueDeclare(queueName, false, isDurable, false, false, null, null, null, null, null);
            messageBus.Advanced.Bind(exchange, queue, "#");
        }

        public void DeclareExchange(string exchangeName)
        {
            messageBus.Advanced.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }
    }
}