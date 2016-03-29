using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddAffordabilities(IApplicationStateMachine applicationStateMachine, IEnumerable<ApplicantModel> applicants)
        {
            var applicationNumber = applicationStateMachine.ApplicationNumber;

            foreach (var applicant in applicants)
            {
                if (applicationStateMachine.ClientCollection.Keys.Contains(applicant.IDNumber))
                {
                    var clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];
                    AddApplicantAffordability(applicant, clientKey, applicationNumber);
                }
            }
        }

        public void AddApplicantAffordability(ApplicantModel applicant, int clientKey, int applicationNumber)
        {
            var commandCorrelationId = combGuidGenerator.Generate();
            try
            {
                var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, commandCorrelationId);
                var clientAffordabilityAssessment = new List<Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel>();
                foreach (var affordability in applicant.Affordabilities)
                {
                    clientAffordabilityAssessment.Add(new Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel(affordability.AffordabilityType,
                        (double)affordability.Amount, affordability.Description));
                }

                var model = new Services.Interfaces.ApplicationDomain.Models.ApplicantAffordabilityModel(clientAffordabilityAssessment, clientKey, applicationNumber);
                var addApplicantAffordabilitiesCommand = new AddApplicantAffordabilitiesCommand(model);
                var serviceMessages = this.applicationDomainService.PerformCommand(addApplicantAffordabilitiesCommand, serviceRequestMetadata);
                CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.AffordabilityDetailCaptured);
            }
            catch (Exception runtimeException)
            {
                var identityNumber = String.Empty;
                if (applicationStateMachine.ClientCollection.Any(x => x.Value == clientKey))
                {
                    identityNumber = applicationStateMachine.ClientCollection.First(x => x.Value == clientKey).Key;
                }
                string friendlyErrorMessage = String.Format("The income/expenditure assessment could not be added for applicant with ID Number: {0}.", identityNumber);
                HandleNonCriticalException(runtimeException, friendlyErrorMessage, commandCorrelationId, applicationStateMachine);
            }
        }
    }
}