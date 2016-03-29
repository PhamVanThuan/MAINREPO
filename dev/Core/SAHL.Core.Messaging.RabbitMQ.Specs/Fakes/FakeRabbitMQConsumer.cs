using RabbitMQ.Client.Events;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeRabbitMQConsumer : IRabbitMQConsumer
    {
        private BasicDeliverEventArgs result;

        public FakeRabbitMQConsumer(BasicDeliverEventArgs result)
        {
            this.result = result;
        }

        public bool Dequeue(int millisecondsTimeout, out BasicDeliverEventArgs result)
        {
            result = this.result;
            this.DequeueTimes += 1;
            return true;
        }

        public int DequeueTimes
        {
            get;
            protected set;
        }
    }
}