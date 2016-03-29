namespace DomainService2.Specs.Workflow.Origination.FurtherLending.CheckSuperLoOptOutRulesCommandHandlerSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(CheckSuperLoOptOutRulesCommandHandler))]
    public class When_an_application_exists : RuleSetDomainServiceSpec<CheckSuperLoOptOutRulesCommand, CheckSuperLoOptOutRulesCommandHandler>
    {
        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            IApplication application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckSuperLoOptOutRulesCommand(applicationKey, false);
            handler = new CheckSuperLoOptOutRulesCommandHandler(commandHandler, applicationRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase("SuperLoOptOutCheck");
        };
    }
}