using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.DebtCounselling.CheckSendToLitigationRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckSendToLitigationRulesCommandHandler))]
    public class When_a_debtcounselling_case_does_not_exist : RuleSetDomainServiceSpec<CheckSendToLitigationRulesCommand, CheckSendToLitigationRulesCommandHandler>
    {
        protected static IDebtCounsellingRepository debtCounsellingRepository;
        protected const string ruleSetName = "DC - Send To Litigation";
        protected static IDebtCounselling debtCounselling;

        // Arrange
        Establish context = () =>
        {
            int debtCounsellingKey = 1;

            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            debtCounselling = null;
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);

            command = new CheckSendToLitigationRulesCommand(debtCounsellingKey, false);
            handler = new CheckSendToLitigationRulesCommandHandler(commandHandler, debtCounsellingRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };
    }
}