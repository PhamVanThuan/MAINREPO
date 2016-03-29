using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckValuationInOrder
{
    [Subject(typeof(CheckValuationInOrderRulesCommandHandler))]
    public class When_the_property_valuation_does_not_exist : RuleSetDomainServiceSpec<CheckValuationInOrderRulesCommand, CheckValuationInOrderRulesCommandHandler>
    {
        private const string ruleSet = RuleSets.ValuationsValuationInOrder;

        Establish context = () =>
            {
                IValuation valuation = null;

                IPropertyRepository propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

                command = new CheckValuationInOrderRulesCommand(1, true);
                handler = new CheckValuationInOrderRulesCommandHandler(commandHandler, propertyRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_rule_parameters_with_null_parameters = () =>
            {
                command.RuleParameters[0].ShouldBeNull();
            };

        It should_set_workflow_ruleset = () =>
            {
                command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
            };
    }
}