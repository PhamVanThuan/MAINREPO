using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void ConditionallyPriceNewBusinessApplication(IApplicationStateMachine applicationStateMachine)
        {
            if (applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.ApplicationEmploymentDetermined) 
                && applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.ApplicationHouseHoldIncomeDetermined))
            {
                var applicationNumber = applicationStateMachine.ApplicationNumber;

                Guid correlationId = combGuidGenerator.Generate();
                var serviceMessages = SystemMessageCollection.Empty();

                try
                {
                    var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, correlationId);
                    var priceNewBusinessApplicationCommand = new PriceNewBusinessApplicationCommand(applicationNumber);
                    serviceMessages = this.financialDomainService.PerformCommand(priceNewBusinessApplicationCommand, serviceRequestMetadata);
                }
                catch (Exception runtimeException)
                {
                    serviceMessages.AddMessage(new SystemMessage(runtimeException.ToString(), SystemMessageSeverityEnum.Error));
                }

                CheckForCriticalErrors(applicationStateMachine, correlationId, serviceMessages);
            }
        }
    }
}