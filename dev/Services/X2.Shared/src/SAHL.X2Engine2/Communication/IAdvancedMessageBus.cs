//using SAHL.Core.X2.Messages;
//using System;

//namespace SAHL.X2Engine2.Communication
//{
//    /// <summary>
//    /// Abstraction of advanvced message bus functionality.
//    /// </summary>
//    public interface IMessageBusAdvanced
//    {
//        void Initialise();

//        void Publish<TMessage>(IX2RouteEndpoint routeEndpoint, TMessage message) where TMessage : IX2Message;

//        void SubscribeWithTopic<TMessage>(IX2RouteEndpoint routeEndpoint, Action<TMessage> messageHandler) where TMessage : class, IX2Message;

//        void Subscribe<TMessage>(Action<TMessage> messageHandler) where TMessage : class, IX2Message;

//        void Teardown();
//    }
//}