using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckSendOfferRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendOfferRulesCommandHandler))]
    public class When_the_application_does_not_exist : RuleSetDomainServiceSpec<CheckSendOfferRulesCommand, CheckSendOfferRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Send Offer";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            IApplication application = null;
            applicationRepository = An<IApplicationRepository>();

            command = new CheckSendOfferRulesCommand(1, false);
            handler = new CheckSendOfferRulesCommandHandler(applicationRepository, commandHandler);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };

    }
}
