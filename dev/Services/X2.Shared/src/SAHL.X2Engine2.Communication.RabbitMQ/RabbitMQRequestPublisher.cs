using RabbitMQ.Client;
using SAHL.Core.Messaging.RabbitMQ;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Communication.RabbitMQ
{
    public class RabbitMQRequestPublisher : IX2RequestPublisher
    {
        private IQueuePublisher queuePublisher;

        public RabbitMQRequestPublisher(IQueuePublisher queuePublisher)
        {
            this.queuePublisher = queuePublisher;
        }

        public void Publish<TRequest>(IX2RouteEndpoint routeEndpoint, TRequest request) where TRequest : class, Core.Messaging.Shared.IMessage
        {
            if (request.GetType() == typeof(X2Request))
            {
                throw new X2PublishException("You may not publish base x2 requests");
            }

            if (routeEndpoint == null)
            {
                return;
            }

            IX2Request x2request = request as IX2Request;
            this.queuePublisher.Publish<X2WrappedRequest>(routeEndpoint.ExchangeName, "#", new X2WrappedRequest(x2request), true, ExchangeType.Direct);
        }
    }
}