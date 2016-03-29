using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.Origination.Credit.CheckEmploymentTypeConfirmedRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckEmploymentTypeConfirmedRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckEmploymentTypeConfirmedRuleCommand, CheckEmploymentTypeConfirmedRuleCommandHandler>
    {
        private static string ruleName = "CheckEmploymentTypeConfirmed";

        Establish context = () =>
        {
            command = new CheckEmploymentTypeConfirmedRuleCommand(Param.IsAny<long>(), false);
            handler = new CheckEmploymentTypeConfirmedRuleCommandHandler(commandHandler);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(long));
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}
