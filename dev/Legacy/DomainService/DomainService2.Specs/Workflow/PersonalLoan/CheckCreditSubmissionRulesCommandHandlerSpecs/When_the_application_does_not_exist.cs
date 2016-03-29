using DomainService2.Workflow.PersonalLoan;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckCreditSubmissionRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditSubmissionRulesCommandHandler))]
    public class When_the_application_does_not_exist : RuleSetDomainServiceSpec<CheckCreditSubmissionRulesCommand, CheckCreditSubmissionRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Application in Order";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();

                command = new CheckCreditSubmissionRulesCommand(1, false);
                handler = new CheckCreditSubmissionRulesCommandHandler(applicationRepository, commandHandler);
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
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };
    }
}