using RabbitMQ.Client;
using SAHL.Core.Messaging.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class RabbitMQResponsePublisher : IX2ResponsePublisher
    {
        private IQueuePublisher queuePublisher;

        public RabbitMQResponsePublisher(IQueuePublisher queuePublisher)
        {
            this.queuePublisher = queuePublisher;
        }

        public void Publish<T>(IX2RouteEndpoint routeEndpoint, T response) where T : class, Core.X2.Messages.IX2Message
        {
            this.queuePublisher.Publish<T>(routeEndpoint.ExchangeName, "#", response, true, ExchangeType.Direct);
        }
    }
}
