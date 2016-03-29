using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<DomiciliumAddressLinkedToApplicantEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(DomiciliumAddressLinkedToApplicantEvent domiciliumAddressLinkedToApplicantEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, domiciliumAddressLinkedToApplicantEvent.Id);
        }
    }
}