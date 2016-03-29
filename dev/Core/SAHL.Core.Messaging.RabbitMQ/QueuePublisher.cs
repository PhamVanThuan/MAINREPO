using Newtonsoft.Json;
using RabbitMQ.Client;
using SAHL.Core.Messaging.Shared;
using SAHL.Core.Serialisation;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueuePublisher : IQueuePublisher
    {
        private IQueueConnection queueConnection;
        private IQueueConnectionFactory connectionFactory;

        public QueuePublisher(IQueueConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            this.queueConnection = this.connectionFactory.CreateConnection();
            this.queueConnection.Connect();
        }

        public void Publish<T>(string exchangeName, string routingKey, T message, bool durable = true, string exchangeType = ExchangeType.Direct) where T : class, IMessage
        {
            var model = queueConnection.CreateModel();
            model.ExchangeDeclare(exchangeName, exchangeType, durable);
            var properties = model.CreateBasicProperties();
            properties.DeliveryMode = 2;

            string messageString = JsonConvert.SerializeObject(message, JsonSerialisation.Settings);
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(messageString);
            model.BasicPublish(exchangeName, routingKey, properties, messageBodyBytes);
        }
    }
}