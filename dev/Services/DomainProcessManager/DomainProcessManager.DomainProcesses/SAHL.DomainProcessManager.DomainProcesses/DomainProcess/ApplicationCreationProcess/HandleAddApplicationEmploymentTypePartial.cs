using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>,
       IDomainProcessEvent<ApplicationEmploymentTypeSetEvent>
       where T : ApplicationCreationModel
    {
        public void Handle(ApplicationEmploymentTypeSetEvent applicationEmploymentTypeSetEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, applicationEmploymentTypeSetEvent.Id);

            ConditionallyPriceNewBusinessApplication(applicationStateMachine);
        }

        public void HandleException(ApplicationEmploymentTypeSetEvent applicationEmploymentTypeSetEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain process failed after setting application employment.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}