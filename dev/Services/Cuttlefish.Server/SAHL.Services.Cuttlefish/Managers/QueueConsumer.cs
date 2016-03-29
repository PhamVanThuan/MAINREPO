using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SAHL.Core.Logging;
using System;
using System.IO;
using System.Text;

namespace SAHL.Services.Cuttlefish.Managers
{
    public class QueueConsumer : IQueueConsumer
    {
        private IQueueConnectionFactory queueInstanceFactory;
        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueConsumer(IQueueConnectionFactory queueInstanceFactory, string hostName, string exchangeName, string queueName, string username, string password, Action<string> workAction, Func<bool> shouldCancel, ILogger logger, ILoggerSource loggerSource)
        {
            this.QueueServerName = hostName;
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.Username = username;
            this.Password = password;
            this.ShouldCancel = shouldCancel;
            this.WorkAction = workAction;
            this.queueInstanceFactory = queueInstanceFactory;
            this.logger = logger;
            this.loggerSource = loggerSource;
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
            while (true)
            {
                try
                {
                    using (var connection = queueInstanceFactory.InitialiseConnection(this.QueueServerName, this.ExchangeName, this.QueueName, this.Username, this.Password))
                    {
                        while (!this.ShouldCancel())
                        {
                            var ea = (BasicDeliverEventArgs)connection.Dequeue();
                            if (ea != null)
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);

                                try
                                {
                                    this.WorkAction(message);
                                    connection.SendAck(ea.DeliveryTag);
                                }
                                catch (Exception ex)
                                {
                                    connection.SendNack(ea.DeliveryTag, true);
                                    this.logger.LogErrorWithException(this.loggerSource, "cuttlefish", "Consume", "Exception while consuming message Nacked", ex);
                                }
                            }
                        }
                        return;
                    }
                }
                catch (BrokerUnreachableException brokerUnreachableException)
                {
                    Console.WriteLine(string.Format("Error consuming from queue BE: {0}", brokerUnreachableException.Message));
                }
                catch (IOException ioException)
                {
                    Console.WriteLine(string.Format("Error consuming from queue: {0}", ioException.Message));
                }
                catch (Exception ex)
                {
                    this.logger.LogErrorWithException(this.loggerSource, "cuttlefish", "Consume", "Unhandled exception while consuming, shutting down.", ex);
                    throw;
                }

                // we should wait a bit here before trying to reconnect
            }
        }
    }
}