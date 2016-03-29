using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<DebitOrderAddedToApplicationEvent>
       where T : ApplicationCreationModel
    {
        public void Handle(DebitOrderAddedToApplicationEvent debitOrderAddedToApplicationEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, debitOrderAddedToApplicationEvent.Id);
        }
    }
}