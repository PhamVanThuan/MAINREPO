using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationAffordabilityAssessment
{
    public class when_rules_return_messages : WithCoreFakes
    {
        private static AddApplicationAffordabilityAssessmentCommand command;
        private static AddApplicationAffordabilityAssessmentCommandHandler handler;

        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;
        private static IADUserManager aduserManager;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();
            aduserManager = An<IADUserManager>();
            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();

            affordabilityAssessmentModel = new AffordabilityAssessmentModel();

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AffordabilityAssessmentModel>()))
                .Callback<ISystemMessageCollection>(a => { a.AddMessage(new SystemMessage("The rule failed.", SystemMessageSeverityEnum.Error)); });

            command = new AddApplicationAffordabilityAssessmentCommand(affordabilityAssessmentModel);
            handler = new AddApplicationAffordabilityAssessmentCommandHandler(domainRuleManager, aduserManager, affordabilityAssessmentManager, eventRaiser);
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
            affordabilityAssessmentManager.WasNotToldTo(x => x.CreateAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentModel>(), Param.IsAny<int>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}