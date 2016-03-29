using System;
using System.Collections.Generic;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueueConsumerManager : IDisposable
    {
        void StartConsumers(IEnumerable<QueueConsumerConfiguration> initalConsumerConfiguration);

        void StopAllConsumers();
    }
}