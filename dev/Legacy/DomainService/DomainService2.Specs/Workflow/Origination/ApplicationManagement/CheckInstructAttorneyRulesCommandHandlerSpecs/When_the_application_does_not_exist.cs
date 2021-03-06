﻿using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckInstructAttorneyRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckInstructAttorneyRulesCommandHandler))]
    public class When_the_application_does_not_exist : RuleSetDomainServiceSpec<CheckInstructAttorneyRulesCommand, CheckInstructAttorneyRulesCommandHandler>
    {
        private const string ruleSet = "InstructAttorney";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                IApplication application = null;
                applicationRepository = An<IApplicationRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                command = new CheckInstructAttorneyRulesCommand(1, true);
                handler = new CheckInstructAttorneyRulesCommandHandler(commandHandler, applicationRepository);
                messages = new DomainMessageCollection();
            };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters_with_null_value = () =>
        {
            command.RuleParameters[0].ShouldBeNull();
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}