using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckCreditSubmissionRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditSubmissionRulesCommandHandler))]
    public class When_the_application_exists : RuleSetDomainServiceSpec<CheckCreditSubmissionRulesCommand, CheckCreditSubmissionRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Application in Order";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                IApplication application = An<IApplication>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                command = new CheckCreditSubmissionRulesCommand(1, true);
                handler = new CheckCreditSubmissionRulesCommandHandler(applicationRepository, commandHandler);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_rule_parameters = () =>
            {
                command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
            };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };
    }
}