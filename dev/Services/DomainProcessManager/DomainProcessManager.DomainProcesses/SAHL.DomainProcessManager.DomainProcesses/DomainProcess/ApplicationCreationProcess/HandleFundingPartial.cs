using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<NewBusinessApplicationFundedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(NewBusinessApplicationFundedEvent applicationFundedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, applicationFundedEvent.Id);

            ApplicationCreationReturnDataModel returnData = new ApplicationCreationReturnDataModel(applicationStateMachine.ApplicationNumber);
            this.StartResultData = returnData;

            LinkExternalVendorToApplication();
            CreateWorkflowCase(applicationFundedEvent.Id);

            var applicants = this.DataModel.Applicants;

            AddClientAddresses(applicants);
            AddClientsBankAccountDetails(applicants);
            AddComCorpPropertyDetails();
            AddAffordabilities(applicationStateMachine, applicants);
            AddDeclarations(applicationStateMachine, applicants);
        }

        private void LinkExternalVendorToApplication()
        {
            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

            var command = new LinkExternalVendorToApplicationCommand(
                                  applicationStateMachine.ApplicationNumber
                                , Core.BusinessModel.Enums.OriginationSource.Comcorp
                                , this.DataModel.VendorCode
                          );
            var serviceMessages = applicationDomainService.PerformCommand(command, metadata);

            CheckForNonCriticalErrors(applicationStateMachine, metadata.CommandCorrelationId, serviceMessages, ApplicationState.ApplicationFunded);
        }
    }
}