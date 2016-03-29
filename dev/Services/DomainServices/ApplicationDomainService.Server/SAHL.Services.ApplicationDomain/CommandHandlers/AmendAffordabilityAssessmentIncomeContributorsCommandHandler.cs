using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AmendAffordabilityAssessmentIncomeContributorsCommandHandler 
            : IDomainServiceCommandHandler<AmendAffordabilityAssessmentIncomeContributorsCommand, AffordabilityAssessmentIncomeContributorsAmendedEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;

        public AmendAffordabilityAssessmentIncomeContributorsCommandHandler(IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager, 
                                                                            IServiceCommandRouter serviceCommandRouter, 
                                                                            IEventRaiser eventRaiser)
        {
            this.domainRuleManager = domainRuleManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.eventRaiser = eventRaiser;

            domainRuleManager.RegisterRule(new AffordabilityAssessmentRequiresAtLeastOneIncomeContributorRule());
            domainRuleManager.RegisterRule(new AffordabilityAssessmentContributingApplicantsMustBeGreaterThanZeroRule());
        }

        public ISystemMessageCollection HandleCommand(AmendAffordabilityAssessmentIncomeContributorsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            domainRuleManager.ExecuteRules(messages, command.AffordabilityAssessment);
            if (messages.HasErrors)
            {
                return messages;
            }

            if (command.AffordabilityAssessment.AffordabilityAssessmentStatus == AffordabilityAssessmentStatus.Confirmed)
            {
                CopyAffordabilityAssessmentIncomeContributorsCommand copyAffordabilityAssessmentCommand = 
                                            new CopyAffordabilityAssessmentIncomeContributorsCommand(command.AffordabilityAssessment);
                serviceCommandRouter.HandleCommand<CopyAffordabilityAssessmentIncomeContributorsCommand>(copyAffordabilityAssessmentCommand, metadata);
            }
            else
            {
                UpdateAffordabilityAssessmentIncomeContributorsCommand updateAffordabilityAssessmentCommand = 
                                            new UpdateAffordabilityAssessmentIncomeContributorsCommand(command.AffordabilityAssessment);
                serviceCommandRouter.HandleCommand<UpdateAffordabilityAssessmentIncomeContributorsCommand>(updateAffordabilityAssessmentCommand, metadata);
            }

            DateTime now = DateTime.Now;
            eventRaiser.RaiseEvent(now, new AffordabilityAssessmentIncomeContributorsAmendedEvent(now, command.AffordabilityAssessment),
                                    command.AffordabilityAssessment.Key, (int)GenericKeyType.AffordabilityAssessment, metadata);

            return messages;
        }
    }
}