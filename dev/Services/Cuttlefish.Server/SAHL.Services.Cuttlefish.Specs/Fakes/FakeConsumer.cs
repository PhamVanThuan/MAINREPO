using SAHL.Services.Cuttlefish.Managers;
using System;
using System.Threading;

namespace SAHL.Services.Cuttlefish.Specs.Fakes
{
    public class FakeConsumer : IQueueConsumer
    {

        public FakeConsumer(Action<string> consumerAction, Func<bool> shouldCancel)
        {
            this.WorkAction = consumerAction;
            this.ShouldCancel = shouldCancel;
        }

        public string QueueServerName
        {
            get;
            protected set;
        }

        public string ExchangeName
        {
            get;
            protected set;
        }

        public string QueueName
        {
            get;
            protected set;
        }

        public string Username
        {
            get;
            protected set;
        }

        public string Password
        {
            get;
            protected set;
        }

        public Func<bool> ShouldCancel
        {
            get;
            protected set;
        }

        public Action<string> WorkAction
        {
            get;
            protected set;
        }

        public void Consume()
        {
            while (!this.ShouldCancel())
            {
                this.WorkAction("test");
            }
        }
    }
}