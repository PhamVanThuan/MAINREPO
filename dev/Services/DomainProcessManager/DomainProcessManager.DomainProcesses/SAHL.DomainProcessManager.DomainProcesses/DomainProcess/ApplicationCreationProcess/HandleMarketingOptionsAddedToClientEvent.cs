using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>,
       IDomainProcessEvent<MarketingOptionsAddedEvent>
       where T : ApplicationCreationModel
    {
        public void Handle(MarketingOptionsAddedEvent marketingOptionsAddedEvent, IServiceRequestMetadata metadata)
        {
        }

    }
}
