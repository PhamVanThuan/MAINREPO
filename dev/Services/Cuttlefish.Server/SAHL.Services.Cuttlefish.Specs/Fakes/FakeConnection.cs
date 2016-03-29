using SAHL.Services.Cuttlefish.Managers;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeConnection : IQueueConnection
    {
        private object dequeueResult;
        private bool ackWasCalled = false;
        private bool nackWasCalled = false;
        private bool dequeueWasCalled = false;

        public FakeConnection(object dequeueResult)
        {
            this.dequeueResult = dequeueResult;
        }

        public string QueueServerName
        {
            get;
            set;
        }

        public string ExchangeName
        {
            get;
            set;
        }

        public string QueueName
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public object Dequeue()
        {
            this.dequeueWasCalled = true;
            return this.dequeueResult;
        }

        public void SendAck(ulong deliveryTag)
        {
            this.ackWasCalled = true;
        }

        public void SendNack(ulong deliveryTag, bool requeue)
        {
            this.nackWasCalled = true;
        }

        public bool IsConnected
        {
            get;
            set;
        }

        public void Dispose()
        {
        }

        public bool AckWasCalled
        {
            get
            {
                return this.ackWasCalled;
            }
        }

        public bool NackWasCalled
        {
            get
            {
                return this.nackWasCalled;
            }
        }

        public bool DequeueWasCalled
        {
            get
            {
                return this.dequeueWasCalled;
            }
        }
    }
}