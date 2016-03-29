using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AmendAffordabilityAssessmentIncomeContributors
{
    public class when_rules_return_messages : WithCoreFakes
    {
        private static AmendAffordabilityAssessmentIncomeContributorsCommand command;
        private static AmendAffordabilityAssessmentIncomeContributorsCommandHandler handler;

        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();

            affordabilityAssessmentModel = new AffordabilityAssessmentModel();

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AffordabilityAssessmentModel>()))
                .Callback<ISystemMessageCollection>(a => { a.AddMessage(new SystemMessage("The rule failed.", SystemMessageSeverityEnum.Error)); });

            command = new AmendAffordabilityAssessmentIncomeContributorsCommand(affordabilityAssessmentModel);
            handler = new AmendAffordabilityAssessmentIncomeContributorsCommandHandler(domainRuleManager, serviceCommandRouter, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The rule failed.");
        };

        private It should_not_make_the_applicant_an_income_contributor = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<IServiceCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}