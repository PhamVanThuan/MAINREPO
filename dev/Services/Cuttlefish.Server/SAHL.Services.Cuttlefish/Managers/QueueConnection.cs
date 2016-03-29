using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SAHL.Core.Logging;
using System;

namespace SAHL.Services.Cuttlefish.Managers
{
    public class QueueConnection : IQueueConnection, IDisposable
    {
        private IConnection connection;
        private IModel channel;
        private QueueingBasicConsumer consumer;
        private ILogger logger;
        private ILoggerSource loggerSource;


        public QueueConnection(string queueServerName, string exchangeName, string queueName, string username, string password, ILogger logger, ILoggerSource loggerSource)
        {
            this.QueueServerName = queueServerName;
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.Username = username;
            this.Password = password;
            this.logger = logger;
            this.loggerSource = loggerSource;

            Console.WriteLine(string.Format("Opening rabbit channel to: {0}", this.QueueServerName));
            var factory = new ConnectionFactory() { HostName = queueServerName, UserName = username, Password = password, RequestedHeartbeat = 15 };

            try
            {
                this.connection = factory.CreateConnection();

                channel = connection.CreateModel();

                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                channel.QueueBind(queueName, exchangeName, "#");

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: queueName, noAck: false, consumer: consumer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("Error Creating Rabbit Connection: {0}", ex.Message));
                this.logger.LogErrorWithException(this.loggerSource, this.Username, "QueueInstance Constructor", "Exception while connecting to queue", ex);
                throw;
            }

            Console.WriteLine("Opening rabbit channel... ready to consume");
        }

        public string QueueServerName { get; protected set; }

        public string ExchangeName { get; protected set; }

        public string QueueName { get; protected set; }

        public string Username { get; protected set; }

        public string Password { get; protected set; }

        public object Dequeue()
        {
            BasicDeliverEventArgs result = null;
            bool didDequeue = consumer.Queue.Dequeue(4000, out result);
            return result;
        }

        public void SendAck(ulong deliveryTag)
        {
            channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void SendNack(ulong deliveryTag, bool requeue)
        {
            channel.BasicNack(deliveryTag: deliveryTag, multiple: false, requeue: requeue);
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;
            this.consumer = null;
            this.channel.Dispose();
            this.connection.Dispose();
        }

        public bool IsConnected
        {
            get { return this.connection != null && this.connection.IsOpen && !disposed; }
        }
    }
}