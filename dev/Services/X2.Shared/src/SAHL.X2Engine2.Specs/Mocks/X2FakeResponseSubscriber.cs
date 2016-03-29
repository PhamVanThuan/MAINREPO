using System;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.Core.Messaging.RabbitMQ;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class FakeResponseSubscriber : IX2ResponseSubscriber
    {
        private Action<X2Response> responseCallback;
        private List<QueueConsumerConfiguration> queueConsumers;

        public void Initialise()
        {
            queueConsumers = new List<QueueConsumerConfiguration>();
        }

        public void InvokeCallback(X2Response response)
        {
            responseCallback(response);
        }

        public void Subscribe<TResponse>(Action<TResponse> responseCallback) where TResponse : X2Response
        {
            this.responseCallback = (Action<X2Response>)responseCallback;
        }

        public System.Collections.Generic.List<SAHL.Core.Messaging.RabbitMQ.QueueConsumerConfiguration> GetConsumers()
        {
            return queueConsumers;   
        }
    }
}