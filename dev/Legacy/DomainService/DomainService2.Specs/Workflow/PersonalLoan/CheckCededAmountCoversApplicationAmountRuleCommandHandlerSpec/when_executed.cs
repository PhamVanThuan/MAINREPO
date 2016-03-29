using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckCededAmountCoversApplicationAmountRuleCommandHandlerSpec
{
    [Subject(typeof(CheckCededAmountCoversApplicationAmountRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckCededAmountCoversApplicationAmountRuleCommand, CheckCededAmountCoversApplicationAmountRuleCommandHandler>
    {
        private static string ruleName = "CheckCededAmountCoversApplicationAmount";

        Establish context = () =>
        {
            command = new CheckCededAmountCoversApplicationAmountRuleCommand(1, 1D, false);
            handler = new CheckCededAmountCoversApplicationAmountRuleCommandHandler(commandHandler);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(int));
            command.RuleParameters[1].ShouldBeOfType(typeof(double));
        };

        It should_set_workflow_rule = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}
