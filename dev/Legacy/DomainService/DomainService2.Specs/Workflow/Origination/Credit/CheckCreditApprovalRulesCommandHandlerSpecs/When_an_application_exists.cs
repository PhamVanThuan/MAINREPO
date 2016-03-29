using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.Credit.CheckCreditApprovalRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditApprovalRulesCommandHandler))]
    public class When_an_application_exists : RuleSetDomainServiceSpec<CheckCreditApprovalRulesCommand, CheckCreditApprovalRulesCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = SAHL.Common.RuleSets.CreditApproval;
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckCreditApprovalRulesCommand(applicationKey, false);
            handler = new CheckCreditApprovalRulesCommandHandler(commandHandler, applicationRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}