using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public class AcceptThirdPartyInvoiceStateMachine : IAcceptThirdPartyInvoiceStateMachine
    {
        public Core.SystemMessages.ISystemMessageCollection SystemMessages
        {
            get;
            protected set;
        }

        public void AggregateMessages(Core.SystemMessages.ISystemMessageCollection systemMessages)
        {
            this.SystemMessages.Aggregate(systemMessages);
        }

        public AcceptThirdPartyInvoiceStateMachine()
        {
            SystemMessages = SystemMessageCollection.Empty();
        }
    }
}