using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddApplicationAffordabilityAssessmentCommandHandler 
        : IDomainServiceCommandHandler<AddApplicationAffordabilityAssessmentCommand, ApplicationAffordabilityAssessmentAddedEvent>
    {
        private IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;
        private IADUserManager adUserManager;
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private IEventRaiser eventRaiser;

        public AddApplicationAffordabilityAssessmentCommandHandler(IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager, 
                                                                   IADUserManager adUserManager, 
                                                                   IAffordabilityAssessmentManager affordabilityAssessmentManager, 
                                                                   IEventRaiser eventRaiser)
        {
            this.domainRuleManager = domainRuleManager;
            this.adUserManager = adUserManager;
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
            this.eventRaiser = eventRaiser;

            domainRuleManager.RegisterRule(new AffordabilityAssessmentRequiresAtLeastOneIncomeContributorRule());
            domainRuleManager.RegisterRule(new AffordabilityAssessmentContributingApplicantsMustBeGreaterThanZeroRule());
        }

        public ISystemMessageCollection HandleCommand(AddApplicationAffordabilityAssessmentCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(systemMessageCollection, command.AffordabilityAssessment);
            if (systemMessageCollection.HasErrors)
            {
                return systemMessageCollection;
            }

            int? _ADUserKey = adUserManager.GetAdUserKeyByUserName(metadata.UserName);
            if (_ADUserKey == null || _ADUserKey.Value <= 0)
            {
                systemMessageCollection.AddMessage(new SystemMessage("Failed to retrieve ADUserKey.", SystemMessageSeverityEnum.Error));
                return systemMessageCollection;
            }

            int affordabilityAssessmentKey = affordabilityAssessmentManager.CreateAffordabilityAssessment(command.AffordabilityAssessment, _ADUserKey.Value);

            foreach (int legalEntityKey in command.AffordabilityAssessment.ContributingApplicantLegalEntities)
            {
                affordabilityAssessmentManager.LinkLegalEntityToAffordabilityAssessment(affordabilityAssessmentKey, legalEntityKey);
            }

            affordabilityAssessmentManager.CreateBlankAffordabilityAssessmentItems(affordabilityAssessmentKey, _ADUserKey.Value);

            DateTime now = DateTime.Now;
            eventRaiser.RaiseEvent(now, new ApplicationAffordabilityAssessmentAddedEvent(now, command.AffordabilityAssessment),
                                    affordabilityAssessmentKey, (int)GenericKeyType.AffordabilityAssessment, metadata);

            return systemMessageCollection;
        }
    }
}