﻿using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.DebtCounselling.CheckSendDeclineLetterRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendDeclineLetterRulesCommandHandler))]
    public class When_a_debtcounselling_case_exists : RuleSetDomainServiceSpec<CheckSendDeclineLetterRulesCommand, CheckSendDeclineLetterRulesCommandHandler>
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

            command = new CheckSendDeclineLetterRulesCommand(debtCounsellingKey, false);
            handler = new CheckSendDeclineLetterRulesCommandHandler(commandHandler, debtCounsellingRepository);
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
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.DebtCounsellingProposalDecline);
        };
    }
}