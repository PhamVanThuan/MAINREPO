using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        bool Dequeue(int millisecondsTimeout, out BasicDeliverEventArgs result);
    }
}
