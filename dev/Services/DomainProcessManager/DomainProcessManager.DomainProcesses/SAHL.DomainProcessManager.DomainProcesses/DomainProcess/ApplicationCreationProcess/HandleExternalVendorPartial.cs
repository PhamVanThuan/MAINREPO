using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ExternalVendorLinkedToApplicationEvent>
       where T : ApplicationCreationModel
    {
        public void Handle(ExternalVendorLinkedToApplicationEvent externalVendorLinkedToApplicationEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed, externalVendorLinkedToApplicationEvent.Id);
        }
    }
}