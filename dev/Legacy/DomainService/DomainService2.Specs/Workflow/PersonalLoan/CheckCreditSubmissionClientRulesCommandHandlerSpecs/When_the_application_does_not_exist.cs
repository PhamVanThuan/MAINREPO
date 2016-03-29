using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckCreditSubmissionClientRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditSubmissionClientRulesCommandHandler))]
    public class When_the_application_does_not_exist : RuleSetDomainServiceSpec<CheckCreditSubmissionClientRulesCommand, CheckCreditSubmissionClientRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Application in Order Client";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                IApplicationUnsecuredLending application = An<IApplicationUnsecuredLending>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                IReadOnlyEventList<ILegalEntity> activeClients = new StubReadOnlyEventList<ILegalEntity>(new ILegalEntity[] { null });
                application.WhenToldTo(x => x.ActiveClients).Return(activeClients);

                command = new CheckCreditSubmissionClientRulesCommand(1, false);
                handler = new CheckCreditSubmissionClientRulesCommandHandler(applicationRepository, commandHandler);
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