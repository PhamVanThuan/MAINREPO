using RabbitMQ.Client;
using SAHL.Core.Logging;
using System;
using System.Threading;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConnection : IQueueConnection
    {
        private readonly object connectionLock = new object();
        private readonly object modelCreationLock = new object();

        private IConnection connection;
        private IRabbitMQConnectionFactoryFactory rabbitMQConnectionFactoryFactory;

        private ILogger logger;
        private ILoggerSource loggerSource;

        public QueueConnection(ILogger logger, ILoggerSource loggerSource, IRabbitMQConnectionFactoryFactory rabbitMQConnectionFactoryFactory)
        {

            this.logger = logger;
            this.loggerSource = loggerSource;
            this.rabbitMQConnectionFactoryFactory = rabbitMQConnectionFactoryFactory;
        }

        public Guid ConnectionId { get; protected set; }

        public void Connect()
        {
            var factory = this.rabbitMQConnectionFactoryFactory.CreateFactory();

            if (factory != null)
            {
                this.connection = factory.CreateConnection();
            }
            else
            {
                this.logger.LogError(this.loggerSource, "", "RabbitMQConnection.Connect", "Could not obtain connection factory for RabbitMQ server.");
                throw new Exception("Could not obtain connection factory for RabbitMQ server.");
            }
            // this must be outside so that even if there has been no connection the ConnectionId will change on reconnect triggerring a reconnect from consumers
            this.ConnectionId = Guid.NewGuid();
        }

        public bool Reconnect()
        {
            if (Monitor.TryEnter(this.connectionLock, 1000))
            {
                try
                {
                    if (this.connection == null || !this.connection.IsOpen)
                    {
                        while (this.connection == null || !this.connection.IsOpen)
                        {
                            try
                            {
                                this.logger.LogInfo(this.loggerSource, "", "RabbitMQConnection.Reconnect", "Reconnecting.");
                                this.Connect();
                            }
                            catch(Exception ex)
                            {
                                // catch and eat in case the connection is still down and throws, so we can try again later
                                this.logger.LogErrorWithException(this.loggerSource, "", "RabbitMQConnection.Reconnect", "Reconnecting failed.", ex);
                            }

                            if (this.connection == null || !this.connection.IsOpen)
                            {
                                // wait a second before trying again
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(this.connectionLock);
                }
            }

            return this.connection != null ? this.connection.IsOpen : false;
        }

        public IRabbitMQModel CreateModel()
        {
            if (Monitor.TryEnter(this.modelCreationLock, 1000))
            {
                try
                {
                    return this.connection != null ? new RabbitMQModel(this.connection.CreateModel()) : null;
                }
                finally
                {
                    Monitor.Exit(this.modelCreationLock);
                }
            }
            else
            {
                return null;
            }
        }

        public bool IsConnected()
        {
            return this.connection != null && this.connection.IsOpen;
        }

        public void Dispose()
        {
            try
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                    this.connection.Dispose();
                }
            }
            catch
            {
                // this is a remotely connected resource, disposing can throw if the connection has dropped, we don't care as long as we have freed up our side
            }
            this.connection = null;
        }
    }
}