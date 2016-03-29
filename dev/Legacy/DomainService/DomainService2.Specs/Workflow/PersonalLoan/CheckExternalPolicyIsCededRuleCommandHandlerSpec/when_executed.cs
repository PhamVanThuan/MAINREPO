using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckExternalPolicyIsCededRuleCommandHandlerSpec
{
    [Subject(typeof(CheckExternalPolicyIsCededRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckExternalPolicyIsCededRuleCommand, CheckExternalPolicyIsCededRuleCommandHandler>
    {
        private static string ruleName = "CheckExternalPolicyIsCeded";

        Establish context = () =>
        {
            command = new CheckExternalPolicyIsCededRuleCommand(1, false);
            handler = new CheckExternalPolicyIsCededRuleCommandHandler(commandHandler);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(int));
        };

        It should_set_workflow_rule = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}
