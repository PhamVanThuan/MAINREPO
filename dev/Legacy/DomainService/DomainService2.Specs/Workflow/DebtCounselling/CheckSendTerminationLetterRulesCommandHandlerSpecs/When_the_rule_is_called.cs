using System;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.DebtCounselling.CheckSendTerminationLetterRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendTerminationLetterRulesCommandHandler))]
    public class When_the_rule_is_called : RuleSetDomainServiceSpec<CheckSendTerminationLetterRulesCommand, CheckSendTerminationLetterRulesCommandHandler>
    {
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounselling;

        // Arrange
        Establish context = () =>
        {
            int debtCounsellingKey = 1;

            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = An<IDebtCounselling>();
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            command = new CheckSendTerminationLetterRulesCommand(debtCounsellingKey, false);
            handler = new CheckSendTerminationLetterRulesCommandHandler(commandHandler, debtCounsellingRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(IDebtCounselling));
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.DebtCounsellingSendTerminationLetter);
        };
    }
}