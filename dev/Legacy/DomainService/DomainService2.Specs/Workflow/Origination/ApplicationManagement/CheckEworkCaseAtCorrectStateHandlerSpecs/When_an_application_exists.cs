using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckEworkCaseAtCorrectState
{
    [Subject(typeof(CheckEWorkAtCorrectStateRuleCommandHandler))]
    public class When_an_application_exists : RuleDomainServiceSpec<CheckEWorkAtCorrectStateRuleCommand, CheckEWorkAtCorrectStateRuleCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = "CheckEWorkInResubmitted";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckEWorkAtCorrectStateRuleCommand(applicationKey, false);
            handler = new CheckEWorkAtCorrectStateRuleCommandHandler(commandHandler, applicationRepository);
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
        It should_set_workflow_rule = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}