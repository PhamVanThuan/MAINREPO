using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckEworkCaseAtCorrectState
{
    [Subject(typeof(CheckEWorkAtCorrectStateRuleCommand))]
    public class When_an_application_does_not_exist : RuleDomainServiceSpec<CheckEWorkAtCorrectStateRuleCommand, CheckEWorkAtCorrectStateRuleCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected const string ruleName = "CheckEWorkInResubmitted";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 0;

            applicationRepository = An<IApplicationRepository>();
            application = null;
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckEWorkAtCorrectStateRuleCommand(applicationKey, false);
            handler = new CheckEWorkAtCorrectStateRuleCommandHandler(commandHandler, applicationRepository);
        };

        // Act
        Because of = () =>
        {
            Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        // Assert
        It should_set_workflow_rulename = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}