using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<NewBusinessApplicationPricedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(NewBusinessApplicationPricedEvent applicationPricedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, applicationPricedEvent.Id);

            if (applicationStateMachine.IsInState(ApplicationState.ApplicationPriced))
            {
                var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

                var command = new FundNewBusinessApplicationCommand(applicationStateMachine.ApplicationNumber);
                var serviceMessages = this.financialDomainService.PerformCommand(command, metadata);

                CheckForCriticalErrors(applicationStateMachine, metadata.CommandCorrelationId, serviceMessages);
            }
        }
        public void HandleException(NewBusinessApplicationPricedEvent applicationPricedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after pricing application.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }

    }
}