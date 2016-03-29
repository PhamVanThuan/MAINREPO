using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public class QueueConsumer : SAHL.Services.Cuttlefish.Worker.Shared.IQueueConsumer
    {
        private string hostName;
        private string exchangeName;
        private string queueName;
        private string userName;
        private string password;
        private Action<string> workAction;

        public QueueConsumer(string hostName, string exchangeName, string queueName, string userName, string password, Action<string> workAction)
        {
            this.hostName = hostName;
            this.exchangeName = exchangeName;
            this.queueName = queueName;
            this.userName = userName;
            this.password = password;
            this.workAction = workAction;
        }

        public void Consume()
        {
            var factory = new ConnectionFactory() { HostName = this.hostName, UserName = this.userName, Password = this.password };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: this.queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: this.queueName, noAck: false, consumer: consumer);

                while (true)
                {
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        this.workAction(message);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch
                    {
                        channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                }
            }
        }
    }
}