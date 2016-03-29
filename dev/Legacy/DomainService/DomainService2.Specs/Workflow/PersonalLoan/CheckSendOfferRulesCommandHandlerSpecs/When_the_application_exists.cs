using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckSendOfferRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendOfferRulesCommandHandler))]
    public class When_the_application_exists : RuleSetDomainServiceSpec<CheckSendOfferRulesCommand, CheckSendOfferRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Send Offer";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            IApplication application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckSendOfferRulesCommand(1, true);
            handler = new CheckSendOfferRulesCommandHandler(applicationRepository, commandHandler);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };
    }
}
