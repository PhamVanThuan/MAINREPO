using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.EasyNetQ
{
    public class EasyNetQNodeManagementPublisher : IX2NodeManagementPublisher
    {
        private IMessageBusAdvanced messageBus;

        public EasyNetQNodeManagementPublisher(IMessageBusAdvanced messageBus)
        {
            this.messageBus = messageBus;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class, IX2NodeManagementMessage
        {
            messageBus.Publish(message);
        }

    }
}
