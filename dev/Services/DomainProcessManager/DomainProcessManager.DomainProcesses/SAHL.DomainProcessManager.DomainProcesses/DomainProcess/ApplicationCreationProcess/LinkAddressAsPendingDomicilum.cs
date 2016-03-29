using SAHL.Core.DomainProcess;
using SAHL.Core.Identity;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        protected void LinkAddressAsPendingDomicilum(IApplicationStateMachine applicationStateMachine, IEnumerable<ApplicantModel> applicants)
        {
            foreach (var applicant in applicants)
            {
                var correlationId = combGuidGenerator.Generate();
                var clientKey = 0;
                var clientAddressKey = 0;
                if (!applicationStateMachine.ClientCollection.TryGetValue(applicant.IDNumber, out clientKey) ||
                    !applicationStateMachine.ClientDomicilumAddressCollection.TryGetValue(applicant.IDNumber, out clientAddressKey))
                {
                    continue;
                }
                try
                {
                    var domicilumGuid = combGuidGenerator.Generate();

                    var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, correlationId);

                    ClientAddressAsPendingDomiciliumModel clientAddressAsPendingDomiciliumModel = new ClientAddressAsPendingDomiciliumModel(clientAddressKey, clientKey);
                    var command = new AddClientAddressAsPendingDomiciliumCommand(clientAddressAsPendingDomiciliumModel, domicilumGuid);

                    var messages = clientDomainService.PerformCommand(command, serviceRequestMetadata);

                    CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, messages, ApplicationState.ClientPendingDomiciliumCaptured);
                }
                catch (Exception runtimeException)
                {
                    string friendlyErrorMessage = string.Format("The Pending Domicilium Address could not be saved for Applicant with ID Number:", applicant.IDNumber);
                    HandleNonCriticalException(runtimeException, friendlyErrorMessage, correlationId, applicationStateMachine);
                }
            }
        }
    }
}