using System;
using DomainService2.Workflow.DebtCounselling;
using Machine.Specifications;

namespace DomainService2.Specs.Workflow.DebtCounselling.CheckFortyFiveDayReminderRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckFortyFiveDayReminderRulesCommandHandler))]
    public class When_the_rule_is_called : RuleSetDomainServiceSpec<CheckFortyFiveDayReminderRulesCommand, CheckFortyFiveDayReminderRulesCommandHandler>
    {
        // Arrange
        Establish context = () =>
        {
            int debtCounsellingKey = 1;

            command = new CheckFortyFiveDayReminderRulesCommand(debtCounsellingKey, false);
            handler = new CheckFortyFiveDayReminderRulesCommandHandler(commandHandler);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(Int32));
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.DebtCounselling45DayReminder);
        };
    }
}