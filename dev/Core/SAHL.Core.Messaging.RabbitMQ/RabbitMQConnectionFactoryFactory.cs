using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class RabbitMQConnectionFactoryFactory : IRabbitMQConnectionFactoryFactory
    {
        private IMessageBusConfigurationProvider messageBusConfigurationProvider;

        public RabbitMQConnectionFactoryFactory(IMessageBusConfigurationProvider messageBusConfigurationProvider)
        {
            this.messageBusConfigurationProvider = messageBusConfigurationProvider;
        }

        public IRabbitMQConnectionFactory CreateFactory()
        {
            return new RabbitMQConnectionFactory(
                this.messageBusConfigurationProvider.HostName,
                this.messageBusConfigurationProvider.UserName,
                this.messageBusConfigurationProvider.Password,
                5);
        }
    }
}
