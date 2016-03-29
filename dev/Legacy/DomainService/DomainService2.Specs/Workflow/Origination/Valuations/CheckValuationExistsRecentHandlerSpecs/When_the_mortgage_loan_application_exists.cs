using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CheckValuationExistsRecent
{
    [Subject(typeof(CheckValuationExistsRecentRulesCommandHandler))]
    public class When_the_mortgage_loan_application_exists : RuleSetDomainServiceSpec<CheckValuationExistsRecentRulesCommand, CheckValuationExistsRecentRulesCommandHandler>
    {
        private static IApplicationMortgageLoan application;
        private const string ruleSet = RuleSets.ValuationsAutoValuation;

        Establish context = () =>
            {
                application = An<IApplicationMortgageLoan>();

                IApplicationRepository applicationRepository = An<IApplicationRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                command = new CheckValuationExistsRecentRulesCommand(1, true);
                handler = new CheckValuationExistsRecentRulesCommandHandler(commandHandler, applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_rule_parameters = () =>
            {
                command.RuleParameters[0].Equals(application);
            };

        It should_set_workflow_ruleset = () =>
            {
                command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
            };
    }
}