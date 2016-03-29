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
    public class When_the_application_exists : RuleSetDomainServiceSpec<CheckCreditSubmissionClientRulesCommand, CheckCreditSubmissionClientRulesCommandHandler>
    {
        private static string ruleSetName = "PersonalLoan - Application in Order Client";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                IApplicationUnsecuredLending application = An<IApplicationUnsecuredLending>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                ILegalEntity legalEntity = An<ILegalEntity>();
                IReadOnlyEventList<ILegalEntity> activeClients = new StubReadOnlyEventList<ILegalEntity>(new ILegalEntity[] { legalEntity });
                application.WhenToldTo(x => x.ActiveClients).Return(activeClients);

                command = new CheckCreditSubmissionClientRulesCommand(1, true);
                handler = new CheckCreditSubmissionClientRulesCommandHandler(applicationRepository, commandHandler);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_rule_parameters = () =>
            {
                command.RuleParameters[0].ShouldBeOfType(typeof(ILegalEntity));
            };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSetName);
        };
    }
}