using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckCanEmailPersonalLoanApplicationRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckCanEmailPersonalLoanApplicationRuleCommandHandler))]
    public class When_executed : RuleDomainServiceSpec<CheckCanEmailPersonalLoanApplicationRuleCommand, CheckCanEmailPersonalLoanApplicationRuleCommandHandler>
    {
        private static string ruleName = "CheckCanEmailPersonalLoanApplication";

        Establish context = () =>
            {
                command = new CheckCanEmailPersonalLoanApplicationRuleCommand(1, false);
                handler = new CheckCanEmailPersonalLoanApplicationRuleCommandHandler(commandHandler);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_rule_parameters = () =>
            {
                command.RuleParameters[0].ShouldBeOfType(typeof(int));
            };

        It should_set_workflow_ruleset = () =>
            {
                command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
            };
    }
}