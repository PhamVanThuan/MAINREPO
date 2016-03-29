using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<UnconfirmedSalariedEmploymentAddedToClientEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(UnconfirmedSalariedEmploymentAddedToClientEvent unconfirmedSalariedEmploymentAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;

            applicationStateMachine.TriggerEmploymentAdded(unconfirmedSalariedEmploymentAddedEvent.Id, unconfirmedSalariedEmploymentAddedEvent.EmploymentKey);

            PerformOperationsFollowingEmploymentAddition(applicationStateMachine);
        }
        public void HandleException(UnconfirmedSalariedEmploymentAddedToClientEvent unconfirmedSalariedEmploymentAddedEvent, IServiceRequestMetadata serviceRequestMetadata, 
            Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain Process failed after adding salaried employment.",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}