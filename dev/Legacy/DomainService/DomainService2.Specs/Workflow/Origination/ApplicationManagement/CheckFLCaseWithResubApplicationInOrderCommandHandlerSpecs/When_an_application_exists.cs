using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckFLCaseWithResubApplicationInOrderCommandHandlerSpecs
{
    public class When_an_application_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static CheckFLCaseWithResubApplicationInOrderCommand command;
        protected static CheckFLCaseWithResubApplicationInOrderCommandHandler commandHandler;
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = "AM - FLCase with Resub - Application In Order";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckFLCaseWithResubApplicationInOrderCommand(applicationKey, false);
            commandHandler = new CheckFLCaseWithResubApplicationInOrderCommandHandler(applicationRepository);
        };

        // Act
        Because of = () =>
        {
            commandHandler.Handle(messages, command);
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