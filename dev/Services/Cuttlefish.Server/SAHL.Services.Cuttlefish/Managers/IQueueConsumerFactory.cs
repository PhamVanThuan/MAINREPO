using System;
using System.Threading;

namespace SAHL.Services.Cuttlefish.Managers
{
    public interface IQueueConsumerFactory
    {
        IQueueConsumer CreateQueueConsumer(string queueExchangeName, string queueName, Action<string> consumerAction, Func<bool> shouldCancel);
    }
}