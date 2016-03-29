using SAHL.Core.Data;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice
{
    public interface IAcceptThirdPartyInvoiceStateMachine : IDataModel
    {
        ISystemMessageCollection SystemMessages { get; }

        void AggregateMessages(ISystemMessageCollection systemMessages);

    }
}
