using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<UnconfirmedSalaryDeductionEmploymentAddedToClientEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(UnconfirmedSalaryDeductionEmploymentAddedToClientEvent unconfirmedSalaryDeductionEmploymentAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;

            applicationStateMachine.TriggerEmploymentAdded(unconfirmedSalaryDeductionEmploymentAddedEvent.Id, unconfirmedSalaryDeductionEmploymentAddedEvent.EmploymentKey);

            PerformOperationsFollowingEmploymentAddition(applicationStateMachine);
        }
        public void HandleException(UnconfirmedSalaryDeductionEmploymentAddedToClientEvent unconfirmedSalaryDeductionEmploymentAddedEvent, IServiceRequestMetadata serviceRequestMetadata,
            Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after adding salary deduction employment.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}