using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;
using System.Linq;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<NaturalPersonClientAddedEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(NaturalPersonClientAddedEvent naturalPersonClientAddedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var applicantIdNumber = serviceRequestMetadata["IdNumberOfAddedClient"];

            var clientKey = naturalPersonClientAddedEvent.ClientKey;

            var linkingGuid = Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
            this.LinkedKeyManager.DeleteLinkedKey(linkingGuid);

            var metadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var applicant = DataModel.Applicants.Single(a => a.IDNumber == applicantIdNumber);

            var addApplicantToApplicationCommand = new AddLeadApplicantToApplicationCommand(combGuidGenerator.Generate(), clientKey, applicationStateMachine.ApplicationNumber, 
                applicant.ApplicantRoleType);
            var serviceMessages = this.applicationDomainService.PerformCommand(addApplicantToApplicationCommand, metadata);

            AddMarketingOptionsToClientCommand(applicant, clientKey, metadata);

            PopulateDictionary(applicationStateMachine.ClientCollection, naturalPersonClientAddedEvent.IDNumber, naturalPersonClientAddedEvent.ClientKey);

            CheckForCriticalErrors(applicationStateMachine, metadata.CommandCorrelationId, serviceMessages);
        }

        public void HandleException(NaturalPersonClientAddedEvent naturalPersonClientAddedEvent, IServiceRequestMetadata serviceRequestMetadata, Exception runtimeException)
        {
            HandleCriticalException(runtimeException, String.Format("Domain Process failed after adding client with ID {0}", naturalPersonClientAddedEvent.IDNumber),
                Guid.Parse(serviceRequestMetadata[DomainProcessManagerGlobals.CommandCorrelationKey]));
        }

        private void AddMarketingOptionsToClientCommand(ApplicantModel applicant, int clientKey, DomainProcessServiceRequestMetadata metadata)
        {
            var marketingOptionsMetaData = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var marketingOptionCollection = new List<MarketingOptionModel>();
            if (applicant.ApplicantMarketingOptions.Any())
            {
                foreach (var marketingOption in applicant.ApplicantMarketingOptions)
                {                    
                    marketingOptionCollection.Add(new MarketingOptionModel((int)marketingOption.MarketingOption, "x2"));
                }
            }
           
            var addMarketingOptionsCommand = new AddMarketingOptionsToClientCommand(marketingOptionCollection, clientKey);
            var messages = this.clientDomainService.PerformCommand(addMarketingOptionsCommand, marketingOptionsMetaData);
            CheckForCriticalErrors(applicationStateMachine, marketingOptionsMetaData.CommandCorrelationId, messages);
        }
    }
}