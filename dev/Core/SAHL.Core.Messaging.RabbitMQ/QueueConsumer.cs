using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SAHL.Core.Logging;
using System;
using System.Text;
using System.Threading;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConsumer : IQueueConsumer
    {
        private IQueueConnection connection;
        private IRabbitMQModel channel;
        private IRabbitMQConsumer consumer;
        private string exchangeName;
        private string queueName;

        private readonly int RECONNECT_WAIT_TIME = 1000;
        private readonly int DEQUEUE_WAIT_TIME = 1000;

        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueConsumer(IQueueConnection connection, string exchangeName, string queueName, Action<string> workAction, Func<bool> shouldCancel, string consumerId, ILogger logger, ILoggerSource loggerSource)
        {
            this.connection = connection;
            this.ShouldCancel = shouldCancel;
            this.WorkAction = workAction;
            this.ConsumerId = consumerId;
            this.exchangeName = exchangeName;
            this.queueName = queueName;
            this.logger = logger;
            this.loggerSource = loggerSource;

            this.SetupConnection();
        }

        public string ConsumerId
        {
            get;
            protected set;
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    return this.connection != null ? this.connection.IsConnected() && !disposed : false;
                }
                catch
                {
                    return false;
                }
            }
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

        public Guid ConnectionId { get; protected set; }

        private void SetupConnection()
        {
            try
            {
                this.ConnectionId = this.connection.ConnectionId;
                this.channel = this.connection.CreateModel();
                this.channel.ExchangeDeclare (exchangeName, ExchangeType.Direct, true);
                this.channel.QueueDeclare(queueName, true, false, false);
                this.channel.QueueBind(queueName, exchangeName, "#");
                this.channel.SetQos(0, 1, false);
                this.consumer = this.channel.SetupConsumerForQueue(queueName, false);
 
            }
            catch (Exception ex)
            {
                this.logger.LogErrorWithException(this.loggerSource, "", "QueueConsumer.SetupConnection", "Failed to setup queues for consumer.", ex);
            }
        }

        private void ClearConnection()
        {
            this.consumer = null;
            if (this.channel != null)
            {
                this.channel.Dispose();
                this.channel = null;
            }
        }

        public void Consume()
        {
            while (true)
            {
                while (!this.ShouldCancel())
                {
                    if (this.IsConnected)
                    {
                        try
                        {
                            if (this.ConnectionId != this.connection.ConnectionId)
                            {
                                this.ClearConnection();
                                this.SetupConnection();
                            }

                            BasicDeliverEventArgs eventArgs = null;
                            if (this.consumer.Dequeue(DEQUEUE_WAIT_TIME, out eventArgs))
                            {
                                if (eventArgs != null)
                                {
                                    try
                                    {
                                        var body = eventArgs.Body;
                                        var message = Encoding.UTF8.GetString(body);

                                        this.WorkAction(message);
                                        this.channel.BasicAck(eventArgs.DeliveryTag, false);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.channel.BasicNack(eventArgs.DeliveryTag, false, true);
                                        this.logger.LogErrorWithException(this.loggerSource, "", "QueueConsumer.Consume", string.Format("Nacking due to exception - {0} - {1}", this.ConsumerId, eventArgs.DeliveryTag), ex);
                                    }
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            // this catch will only happen if there is a conenction failure, so eat it and retry
                            this.logger.LogErrorWithException(this.loggerSource, "", "QueueConsumer.Consume", "RabbitMQ connection failure.", ex);
                        }
                    }
                    else
                    {
                        // reconnect and wait to try later if we can't after 1 second
                        if (!this.connection.Reconnect())
                        {
                            Thread.Sleep(RECONNECT_WAIT_TIME);
                        }
                    }
                }
                return;
            }
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
            this.channel = null;
        }
    }
}