using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckApplicationDebitOrderCollectionRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckApplicationDebitOrderCollectionRuleCommandHandler))]
    public class When_an_application_does_not_exist : RuleDomainServiceSpec<CheckApplicationDebitOrderCollectionRuleCommand, CheckApplicationDebitOrderCollectionRuleCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected const string rule = "ApplicationDebitOrderCollection";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = null;
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckApplicationDebitOrderCollectionRuleCommand(applicationKey, false);
            handler = new CheckApplicationDebitOrderCollectionRuleCommandHandler(commandHandler, applicationRepository);
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
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(rule);
        };
    }
}