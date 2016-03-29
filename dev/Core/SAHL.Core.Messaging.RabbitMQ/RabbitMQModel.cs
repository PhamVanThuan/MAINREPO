using RabbitMQ.Client;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class RabbitMQModel : IRabbitMQModel
    {
        private IModel model;

        public RabbitMQModel(IModel model)
        {
            this.model = model;
        }

        public void ExchangeDeclare(string exchangeName, string type, bool durable)
        {
            this.model.ExchangeDeclare(exchangeName, type, durable);
        }

        public void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            this.model.QueueBind(queueName, exchangeName, routingKey);
        }

        public void QueueDeclare(string queueName, bool durable, bool exclusive, bool autoDelete)
        {
            this.model.QueueDeclare(queueName, durable, exclusive, autoDelete, null);
        }

        public void SetQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            this.model.BasicQos(prefetchSize, prefetchCount, global);
        }

        public IRabbitMQConsumer SetupConsumerForQueue(string queueName, bool noAck)
        {
            var consumer = new QueueingBasicConsumer(this.model);
            this.model.BasicConsume(queueName, noAck, consumer);
            return new RabbitMQConsumer(consumer);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            this.model.BasicAck(deliveryTag, multiple);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            this.model.BasicNack(deliveryTag, multiple, requeue);
        }

        public void Dispose()
        {
            if (this.model != null)
            {
                this.model.Dispose();
                this.model = null;
            }
        }

        public void BasicPublish(string exchangeName, string routingKey, IBasicProperties properties, byte[] messageBodyBytes)
        {
            this.model.BasicPublish(exchangeName, routingKey, properties, messageBodyBytes);
        }

        public IBasicProperties CreateBasicProperties()
        {
            return this.model.CreateBasicProperties();
        }
    }
}