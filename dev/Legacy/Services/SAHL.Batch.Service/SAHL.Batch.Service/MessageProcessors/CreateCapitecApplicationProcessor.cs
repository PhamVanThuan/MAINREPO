using SAHL.Batch.Common;
using SAHL.Batch.Common.Messages;
using SAHL.Batch.Common.ServiceContracts;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.MessageProcessors
{
    public class CreateCapitecApplicationProcessor : IMessageProcessor<CapitecApplicationMessage>
    {
        private ICapitecClientService client;

        public CreateCapitecApplicationProcessor(ICapitecClientService client)
        {
            this.client = client;
        }

        public bool Process(CapitecApplicationMessage message)
        {
            var status = this.client.CreateApplication(message.CapitecApplication, message.Id);
            return status;
        }

        public bool Process<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
