//using SAHL.Core.SystemMessages;
//using SAHL.Core.X2.Messages;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SAHL.X2Engine2.Communication
//{
//    public class InMemoryMessageBus : IMessageBusAdvanced
//    {
//        private Dictionary<Type, List<MessageHandler>> handlerLookup;
//        private ITopicNameBuilder topicNameBuilder;

//        public InMemoryMessageBus(ITopicNameBuilder topicNameBuilder)
//        {
//            this.topicNameBuilder = topicNameBuilder;
//            handlerLookup = new Dictionary<Type, List<MessageHandler>>();
//        }

//        public InMemoryMessageBus()
//            : this(new DefaultTopicNameBuilder())
//        {
//            handlerLookup = new Dictionary<Type, List<MessageHandler>>();
//        }

//        public void Initialise()
//        {
//        }

//        public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : class, IX2Message
//        {
//            AddHandlerToLookup(handler);
//        }

//        public void SubscribeWithTopic<TMessage>(IX2RouteEndpoint routeEndpoint, Action<TMessage> handler) where TMessage : class, IX2Message
//        {
//            var topic = topicNameBuilder.GenerateTopicName(routeEndpoint);
//            AddHandlerToLookup(handler, topic);
//        }

//        public void Publish<TMessage>(IX2RouteEndpoint routeEndpoint, TMessage message) where TMessage : IX2Message
//        {
//            var topic = topicNameBuilder.GenerateTopicName(routeEndpoint);
//            DispatchByType(message, topic);
//        }

//        private void AddHandlerToLookup<TMessage>(Action<TMessage> handler, string topic = null)
//        {
//            List<MessageHandler> handlers;
//            var type = typeof(TMessage);
//            if (!handlerLookup.TryGetValue(type, out handlers))
//            {
//                handlerLookup.Add(type, handlers = new List<MessageHandler>());
//            }
//            if (!handlers.Any(x => x.Equals(handler)))
//            {
//                handlers.Add(new MessageHandler((message) =>
//                {
//                    handler((TMessage)message);
//                }, topic));
//            }
//        }

//        private void DispatchByType(IX2Message message, string topic = null)
//        {
//            var type = message.GetType();
//            do
//            {
//                DispatchByType(message, topic, type);
//                type = type.BaseType;
//            } while (type != null && type != message.GetType());
//        }

//        private void DispatchByType(IX2Message message, string topic, Type type)
//        {
//            List<MessageHandler> list;
//            if (!handlerLookup.TryGetValue(type, out list)) return;
//            foreach (var handler in list.Where(x => x.Topic == null || x.Topic == topic))
//            {
//                handler.HandleCommand(message);
//            }
//        }

//        public void Teardown()
//        {
//            handlerLookup.Clear();
//            handlerLookup = null;
//        }

//        private class MessageHandler
//        {
//            public Action<IX2Message> Handler { get; set; }

//            public string Topic { get; set; }

//            public MessageHandler(Action<IX2Message> handler, string topic)
//            {
//                this.Handler = handler;
//                this.Topic = topic;
//            }

//            public ISystemMessageCollection HandleCommand(IX2Message message)
//            {
//                Handler.Invoke(message);
//                return new SystemMessageCollection();
//            }
//        }
//    }
//}