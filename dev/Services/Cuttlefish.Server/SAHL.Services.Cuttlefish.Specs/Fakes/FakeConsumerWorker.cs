using SAHL.Services.Cuttlefish.Managers;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeConsumerWorker : IQueueConsumerWorker
    {
        public void ProcessMessage(string queueMessage)
        {
        }
    }
}