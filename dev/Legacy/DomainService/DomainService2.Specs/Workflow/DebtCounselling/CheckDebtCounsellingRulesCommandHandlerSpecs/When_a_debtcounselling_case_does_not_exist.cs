using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.CheckDebtCounsellingRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendToLitigationRulesCommandHandler))]
    public class When_a_debtcounselling_case_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static CheckDebtCounsellingRulesCommand command;
        protected static CheckDebtCounsellingRulesCommandHandler commandHandler;
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected static IDebtCounselling debtCounselling;

        // Arrange
        Establish context = () =>
        {
            int debtCounsellingKey = 1;

            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = null;
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            command = new CheckDebtCounsellingRulesCommand(debtCounsellingKey, SAHL.Common.RuleSets.DebtCounsellingSendToLitigation, false);
            commandHandler = new CheckDebtCounsellingRulesCommandHandler(debtCounsellingRepository);
        };

        // Act
        Because of = () =>
        {
            commandHandler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.DebtCounsellingSendToLitigation);
        };
    }
}