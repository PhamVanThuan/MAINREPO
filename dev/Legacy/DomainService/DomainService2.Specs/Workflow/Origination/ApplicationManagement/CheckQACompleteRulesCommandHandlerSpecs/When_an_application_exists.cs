using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckQACompleteRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckQACompleteRulesCommandHandler))]
    public class When_an_application_exists : RuleSetDomainServiceSpec<CheckQACompleteRulesCommand, CheckQACompleteRulesCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = "AM - QA Complete";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
            {
                int applicationKey = 1;

                applicationRepository = An<IApplicationRepository>();
                application = An<IApplication>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                command = new CheckQACompleteRulesCommand(applicationKey, false);
                handler = new CheckQACompleteRulesCommandHandler(commandHandler, applicationRepository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_set_rule_parameters = () =>
            {
                command.RuleParameters[0].Equals(application);
            };

        // Assert
        It should_set_workflow_ruleset = () =>
            {
                command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
            };
    }
}