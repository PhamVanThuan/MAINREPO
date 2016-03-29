using System;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConsumerConfiguration
    {
        public QueueConsumerConfiguration(string exchangeName, string queueName, int initialNumberOfConsumers, Action<string> workAction)
        {
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.InitialNumberOfConsumers = initialNumberOfConsumers;
            this.WorkAction = workAction;
        }

        public string ExchangeName { get; protected set; }

        public string QueueName { get; protected set; }

        public int InitialNumberOfConsumers { get; protected set; }

        public Action<string> WorkAction { get; protected set; }
    }
}