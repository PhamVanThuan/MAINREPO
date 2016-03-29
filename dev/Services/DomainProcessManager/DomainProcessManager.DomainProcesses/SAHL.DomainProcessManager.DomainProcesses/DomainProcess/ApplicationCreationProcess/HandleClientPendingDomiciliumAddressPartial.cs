using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<ClientAddressAsPendingDomiciliumAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(ClientAddressAsPendingDomiciliumAddedEvent clientAddressAsPendingDomiciliumAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var stateMachine = this.ProcessState as IApplicationStateMachine;
            stateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, clientAddressAsPendingDomiciliumAddedEvent.Id);
            Guid correlationId = combGuidGenerator.Generate();

            try
            {
                int clientKey = clientDataManager.GetClientKeyForClientAddress(clientAddressAsPendingDomiciliumAddedEvent.LegalEntityAddressKey);
                var applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(clientAddressAsPendingDomiciliumAddedEvent.ClientDomiciliumKey, stateMachine.ApplicationNumber, clientKey);
                var linkDomiciliumToApplicantCommand = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
                var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, correlationId);
                var serviceMessages = applicationDomainService.PerformCommand(linkDomiciliumToApplicantCommand, metadata);

                CheckForNonCriticalErrors(stateMachine, correlationId, serviceMessages, ApplicationState.ClientPendingDomiciliumCaptured);
            }
            catch (Exception runtimeException)
            {
                var identityNumber = String.Empty;
                if (stateMachine.ClientDomicilumAddressCollection.Any(x => x.Value == clientAddressAsPendingDomiciliumAddedEvent.LegalEntityAddressKey))
                {
                    identityNumber = stateMachine.ClientDomicilumAddressCollection.First(x => x.Value == clientAddressAsPendingDomiciliumAddedEvent.LegalEntityAddressKey).Key;
                }

                string friendlyErrorMessage = String.Format("Could not link domicilium address for applicant with ID Number {0}", identityNumber);
                HandleNonCriticalException(runtimeException, friendlyErrorMessage, correlationId, stateMachine);
            }
        }
    }
}