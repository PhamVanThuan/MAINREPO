using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
       where T : ApplicationCreationModel
    {
        protected void AddApplicationEmploymentType(IApplicationStateMachine stateMachine)
        {
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

            var setApplicationEmploymentTypeCommand = new SetApplicationEmploymentTypeCommand(stateMachine.ApplicationNumber);
            var serviceMessages = this.applicationDomainService.PerformCommand(setApplicationEmploymentTypeCommand, serviceRequestMetadata);

            CheckForCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages);
        }
    }
}