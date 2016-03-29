using EasyNetQ;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Communication;

namespace SAHL.Batch.Common.MessageForwarding
{
    public class ForwardingMessageBus : IForwardingMessageBus
    {
        private IMessageBusConfigurationProvider config;
        private IBus serviceBus;
        private DefaultEasyNetQMessageBusSettings settings;

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpointName">Every consumer must have a unique EP ie rabbitMQ://MachineName/ApplicationName</param>
        public ForwardingMessageBus(IMessageBusConfigurationProvider config)
        {
            this.config = config;
            this.settings = new DefaultEasyNetQMessageBusSettings();
            var connectionString = String.Format("host={0}", this.config.MessageServer);
            if (!String.IsNullOrEmpty(this.config.UserName))
            {
                connectionString = String.Format("host={0};username={1};password={2}", this.config.MessageServer,
                                                                                       this.config.UserName,
                                                                                       this.config.Password);
            }
            serviceBus = RabbitHutch.CreateBus(connectionString, x =>
            {
                if (settings != null)
                {
                    settings.RegisterServices(x);
                }
            });
        }

        public void Publish<T>(T message) where T : class
        {
            if (serviceBus != null)
            {
				serviceBus.Publish<T>(message);
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : class
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
