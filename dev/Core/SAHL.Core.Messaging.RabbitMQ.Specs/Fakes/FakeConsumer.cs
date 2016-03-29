using System;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeConsumer : IQueueConsumer
    {
        private Func<bool> shouldCancel;

        public FakeConsumer(Action<string> workAction, Func<bool> shouldCancel, string consumerId)
        {
            this.ShouldCancel = shouldCancel;
            this.WorkAction = workAction;
            this.ConsumerId = consumerId;
        }

        public bool WasCancelled { get; protected set; }

        public string ConsumerId
        {
            get; set;
        }

        public bool IsConnected
        {
            get; set;
        }

        public Func<bool> ShouldCancel
        {
            get
            {
                return () =>
                {
                    WasCancelled = true;
                    return this.shouldCancel();
                };
            }
            set
            {
                this.shouldCancel = value;
            }
        }

        public Action<string> WorkAction
        {
            get; set;
        }

        public void Consume()
        {
            while (!this.ShouldCancel())
            {

            }
        }
    }
}