using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<UnconfirmedUnemployedEmploymentAddedToClientEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(UnconfirmedUnemployedEmploymentAddedToClientEvent unconfirmedUnemployedEmploymentAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicationStateMachine = this.ProcessState as IApplicationStateMachine;

            applicationStateMachine.TriggerEmploymentAdded(unconfirmedUnemployedEmploymentAddedEvent.Id, unconfirmedUnemployedEmploymentAddedEvent.EmploymentKey);

            PerformOperationsFollowingEmploymentAddition(applicationStateMachine);
        }
    }
}