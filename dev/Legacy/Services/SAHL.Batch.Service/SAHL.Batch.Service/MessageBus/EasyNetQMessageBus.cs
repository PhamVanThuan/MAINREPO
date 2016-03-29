using EasyNetQ;
using SAHL.Batch.Common;
using SAHL.Batch.Common.MessageForwarding;
using SAHL.Communication;
using SAHL.Shared.Messages;
using System;

namespace SAHL.Batch.Service.MessageBus
{
    public class EasyNetQMessageBus : MarshalByRefObject, IDiposableMessageBus
    {
        private IMessageBusConfigurationProvider config;
        private IBus serviceBus;
        private DefaultEasyNetQMessageBusSettings settings;

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpointName">Every consumer must have a unique EP ie rabbitMQ://MachineName/ApplicationName</param>
        public EasyNetQMessageBus(IMessageBusConfigurationProvider config)
        {
            this.config = config;   
            this.settings = new DefaultEasyNetQMessageBusSettings();
            var connectionString = String.Format("host={0};username={1};password={2}", this.config.MessageServer, this.config.UserName, this.config.Password);
            serviceBus = RabbitHutch.CreateBus(connectionString, x =>
            {
                x.Register<ISerializer>(y => { return new SAHL.Communication.JsonSerializer(new TypeNameSerializer()); });
                if (settings != null)
                {
                settings.RegisterServices(x);
                }
            });
        }

        public void Publish<T>(T message) where T : class, SAHL.Shared.Messages.IMessage
        {
            if (serviceBus != null)
            {
				serviceBus.Publish<T>(message);
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : class, SAHL.Shared.Messages.IMessage
        {
            if (serviceBus != null)
            {
                serviceBus.Subscribe(this.config.QueueName, handler);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.serviceBus.Dispose();
                if (this.serviceBus != null)
                {
                    this.serviceBus = null;
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}