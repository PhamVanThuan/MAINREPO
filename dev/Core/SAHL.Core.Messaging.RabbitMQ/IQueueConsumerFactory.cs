using SAHL.Core.Logging;
using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public interface IQueueConsumerFactory
    {
        IQueueConsumer CreateConsumer(IQueueConnection connection, string exchangeName, string queueName, Action<string> workAction, Func<bool> shouldCancel, string consumerId);
    }
}