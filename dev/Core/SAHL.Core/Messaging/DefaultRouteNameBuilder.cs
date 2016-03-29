using System;

namespace SAHL.Core.Messaging
{
    public class DefaultRouteNameBuilder<TMessageRoute> : IMessageRouteNameBuilder<TMessageRoute>
        where TMessageRoute : IMessageRoute
    {
        public string BuildRouteName(IMessageRoute route)
        {
            return String.Format("{0}.{1}", route.ExchangeName, route.QueueName);
        }
    }
}