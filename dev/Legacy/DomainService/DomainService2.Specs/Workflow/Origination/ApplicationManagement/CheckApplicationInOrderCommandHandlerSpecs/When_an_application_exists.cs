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

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckApplicationInOrderCommandHandlerSpecs
{
    public class When_an_application_exists : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static CheckApplicationInOrderCommand command;
        protected static CheckApplicationInOrderCommandHandler commandHandler;
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = "AM - Application In Order";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckApplicationInOrderCommand(applicationKey, false);
            commandHandler = new CheckApplicationInOrderCommandHandler(applicationRepository);
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