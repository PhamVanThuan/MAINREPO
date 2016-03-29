using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckAlteredApprovalStageTransitionRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckAlteredApprovalStageTransitionRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckAlteredApprovalStageTransitionRuleCommand, CheckAlteredApprovalStageTransitionRuleCommandHandler>
    {
        private static string ruleName = "CheckAlteredApprovalStageTransition";

        Establish context = () =>
        {
            command = new CheckAlteredApprovalStageTransitionRuleCommand(1, false);
            handler = new CheckAlteredApprovalStageTransitionRuleCommandHandler(commandHandler);
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
