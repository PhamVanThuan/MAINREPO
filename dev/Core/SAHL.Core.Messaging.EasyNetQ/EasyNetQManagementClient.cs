using EasyNetQ.Management.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.EasyNetQ
{
    public class EasyNetQManagementClient : IMessageBusManagementClient
    {
        private IMessageBusConfigurationProvider messageBusConfigurationProvider;
        private IManagementClient managementClient;

        public EasyNetQManagementClient(IMessageBusConfigurationProvider messageBusConfigurationProvider)
        {
            this.messageBusConfigurationProvider = messageBusConfigurationProvider;
            this.managementClient = new ManagementClient(String.Format("{0}", messageBusConfigurationProvider.HostName), messageBusConfigurationProvider.UserName, messageBusConfigurationProvider.Password);
        }

        public void Dispose()
        {
            if (this.managementClient != null)
            {
                this.managementClient = null;
            }
        }

        public List<string> GetQueuesWithConsumers()
        {
            return this.managementClient.GetQueues().Where(x => x.Consumers > 0).Select(y => y.Name).ToList();
        }
    }
}
