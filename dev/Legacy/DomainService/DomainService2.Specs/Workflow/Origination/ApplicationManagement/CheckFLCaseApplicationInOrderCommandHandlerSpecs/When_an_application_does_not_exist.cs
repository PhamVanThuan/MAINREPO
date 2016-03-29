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

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckFLCaseApplicationInOrderCommandHandlerSpecs
{
    public class When_an_application_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static CheckFLCaseApplicationInOrderCommand command;
        protected static CheckFLCaseApplicationInOrderCommandHandler commandHandler;
        protected static IApplicationRepository applicationRepository;
        protected const string ruleSet = "AM - FLCase - Application In Order";
        protected static IApplication application;

        // Arrange
        Establish context = () =>
        {
            int applicationKey = 1;

            applicationRepository = An<IApplicationRepository>();
            application = null;
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckFLCaseApplicationInOrderCommand(applicationKey, false);
            commandHandler = new CheckFLCaseApplicationInOrderCommandHandler(applicationRepository);
        };

        // Act
        Because of = () =>
        {
            commandHandler.Handle(messages, command);
        };

        // Assert
        It should_set_rule_parameters_with_null_values = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        // Assert
        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}