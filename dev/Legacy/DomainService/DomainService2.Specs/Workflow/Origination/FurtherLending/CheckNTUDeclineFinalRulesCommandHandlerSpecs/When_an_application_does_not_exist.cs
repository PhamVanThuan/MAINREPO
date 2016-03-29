namespace DomainService2.Specs.Workflow.Origination.FurtherLending.CheckNTUDeclineFinalRulesCommandHandlerSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(CheckCanDisburseReadvanceRulesCommandHandler))]
    public class When_an_application_does_not_exist : RuleSetDomainServiceSpec<CheckNTUDeclineFinalRulesCommand, CheckNTUDeclineFinalRulesCommandHandler>
    {
        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            IApplication application = null;
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckNTUDeclineFinalRulesCommand(applicationKey, false);
            handler = new CheckNTUDeclineFinalRulesCommandHandler(commandHandler, applicationRepository);
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
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase("FL - NTU Final/Decline Final");
        };
    }
}