using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>,
     IDomainProcessEvent<ApplicationHouseholdIncomeDeterminedEvent>
     where T : ApplicationCreationModel
    {
        public void Handle(ApplicationHouseholdIncomeDeterminedEvent applicationHouseholdIncomeDeterminedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var stateMachine = this.ProcessState as IApplicationStateMachine;
            stateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, applicationHouseholdIncomeDeterminedEvent.Id);

            ConditionallyPriceNewBusinessApplication(stateMachine);
        }

        public void HandleException(ApplicationHouseholdIncomeDeterminedEvent applicationHouseholdIncomeDeterminedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain process failed after setting application household income.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}