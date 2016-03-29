using SAHL.Core.Messaging.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication
{
    public interface IX2ConsumerConfigurationProvider
    {
        List<QueueConsumerConfiguration> GetConsumers();
    }
}
