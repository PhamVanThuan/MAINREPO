using SAHL.Core.Events;
using System.Linq;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddApplicantAffordabilitiesCommandHandler : IDomainServiceCommandHandler<AddApplicantAffordabilitiesCommand, ApplicantAffordabilitiesAddedEvent>
    {
        private IDomainRuleManager<ApplicantAffordabilityModel> domainRuleManager;
        private IApplicantDataManager applicantDataManager;
        private IEventRaiser eventRaiser;
        private IUnitOfWorkFactory uowFactory;
        private IAffordabilityDataManager affordabilityDataManager;

        public AddApplicantAffordabilitiesCommandHandler(IDomainRuleManager<ApplicantAffordabilityModel> domainRuleManager, IApplicantDataManager applicantDataManager, IEventRaiser eventRaiser, 
            IUnitOfWorkFactory uowFactory, IAffordabilityDataManager affordabilityDataManager)
        {
            this.domainRuleManager = domainRuleManager;
            this.applicantDataManager = applicantDataManager;
            this.eventRaiser = eventRaiser;
            this.uowFactory = uowFactory;
            this.domainRuleManager = domainRuleManager;
            this.affordabilityDataManager = affordabilityDataManager;

            this.domainRuleManager.RegisterRule(new AffordabilityDescriptionRequiredRule(this.affordabilityDataManager));
            this.domainRuleManager.RegisterRule(new AssessmentCanContainOnlyOneOfEachAffordabilityTypeRule());
            this.domainRuleManager.RegisterRule(new ApplicantCannotHaveAnExistingAffordabilityAssessmentRule(this.affordabilityDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddApplicantAffordabilitiesCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                var clientKey = command.ApplicantAffordabilityModel.ClientKey;
                var applicationNumber = command.ApplicantAffordabilityModel.ApplicationNumber;
                var isApplicantOnApplication = applicantDataManager.CheckClientIsAnApplicantOnTheApplication(clientKey, applicationNumber);

                if (!isApplicantOnApplication)
                {
                    messages.AddMessage(new SystemMessage("Applicant does not play a role on application.", SystemMessageSeverityEnum.Error));
                }
                //run rules
                domainRuleManager.ExecuteRules(messages, command.ApplicantAffordabilityModel);

                if (!messages.HasErrors)
                {

                    foreach (var affordabilityModel in command.ApplicantAffordabilityModel.ClientAffordabilityAssessment)
                    {
                        //save affodability
                        affordabilityDataManager.SaveAffordability(affordabilityModel, clientKey, applicationNumber);
                    }

                    //raise event
                    ApplicantAffordabilitiesAddedEvent @event = new ApplicantAffordabilitiesAddedEvent(DateTime.Now, clientKey, applicationNumber, 
                        command.ApplicantAffordabilityModel.ClientAffordabilityAssessment);
                    eventRaiser.RaiseEvent(@event.Date, @event, clientKey, (int)GenericKeyType.LegalEntity, metadata);

                }
                uow.Complete();
            }

            return messages;
        }

    }
}