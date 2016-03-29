using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<LeadApplicantAddedToApplicationEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(LeadApplicantAddedToApplicationEvent applicantAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, applicantAddedEvent.Id);
            this.LinkedKeyManager.DeleteLinkedKey(Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));

            var idNumber = applicationStateMachine.ClientCollection.First(x => x.Value == applicantAddedEvent.ClientKey).Key;
            var addedApplicant = this.DataModel.Applicants.First(x => x.IDNumber == idNumber);
            if (addedApplicant.IncomeContributor)
            {
                var command = new MakeApplicantAnIncomeContributorCommand(applicantAddedEvent.ApplicationRoleKey);
                var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
                var messages = this.applicationDomainService.PerformCommand(command, metadata);
                CheckForCriticalErrors(applicationStateMachine, metadata.CommandCorrelationId, messages);
            }

            bool allLeadApplicantsHaveBeenAdded = applicationStateMachine.AllLeadApplicantsHaveBeenAdded();
            if (allLeadApplicantsHaveBeenAdded)
            {
                foreach (var applicant in DataModel.Applicants)
                {
                    AddClientEmployment(applicant);
                }
            }
        }

        public void HandleException(LeadApplicantAddedToApplicationEvent applicantAddedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, "Domain process failed after adding applicant",
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }
    }
}