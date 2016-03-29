using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
       where T : ApplicationCreationModel
    {
        public void AddApplicationHousholdIncome(IApplicationStateMachine applicationStateMachine)
        {
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

            var determineApplicationHouseholdIncomeCommand = new DetermineApplicationHouseholdIncomeCommand(applicationStateMachine.ApplicationNumber);
            var serviceMessages = this.applicationDomainService.PerformCommand(determineApplicationHouseholdIncomeCommand, metadata);

            CheckForCriticalErrors(applicationStateMachine, metadata.CommandCorrelationId, serviceMessages);
        }
    }
}