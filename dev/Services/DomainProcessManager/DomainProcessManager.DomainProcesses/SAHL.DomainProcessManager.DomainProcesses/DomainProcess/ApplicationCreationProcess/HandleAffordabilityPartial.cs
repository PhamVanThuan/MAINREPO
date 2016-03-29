using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ApplicantAffordabilitiesAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(ApplicantAffordabilitiesAddedEvent applicantAffordabilitiesAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, applicantAffordabilitiesAddedEvent.Id);
        }
    }
}