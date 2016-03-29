using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckValuationInOrder
{
    [Subject(typeof(CheckValuationInOrderRulesCommandHandler))]
    public class When_the_property_valuation_exists : RuleSetDomainServiceSpec<CheckValuationInOrderRulesCommand, CheckValuationInOrderRulesCommandHandler>
    {
        private const string ruleSet = RuleSets.ValuationsValuationInOrder;

        private static IValuation valuation;

        Establish context = () =>
            {
                valuation = An<IValuation>();

                IPropertyRepository propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

                command = new CheckValuationInOrderRulesCommand(1, true);
                handler = new CheckValuationInOrderRulesCommandHandler(commandHandler, propertyRepository);
            };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].Equals(valuation);
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}