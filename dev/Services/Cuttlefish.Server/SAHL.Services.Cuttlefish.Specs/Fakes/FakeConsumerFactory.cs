using SAHL.Services.Cuttlefish.Managers;
using System;
using System.Threading;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeConsumerFactory : IQueueConsumerFactory
    {
        public IQueueConsumer CreateQueueConsumer(string queueExchangeName, string queueName, Action<string> consumerAction, Func<bool> shouldCancel)
        {
            return new FakeConsumer(consumerAction, shouldCancel);
        }
    }
}