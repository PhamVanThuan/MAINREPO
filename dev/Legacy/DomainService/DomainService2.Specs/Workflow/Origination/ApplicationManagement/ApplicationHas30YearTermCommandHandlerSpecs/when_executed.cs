using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.ApplicationHas30YearTermCommandHandlerSpecs
{
    [Subject(typeof(CheckApplicationHas30YearTermRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckApplicationHas30YearTermRuleCommand, CheckApplicationHas30YearTermRuleCommandHandler>
    {
        private static string ruleName = "ApplicationHas30YearTerm";
        private static IApplicationRepository appRepo;
        private static IApplication app;

        Establish context = () =>
        {
            appRepo = An<IApplicationRepository>();
            app = An<IApplication>();
            appRepo.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);
            command = new CheckApplicationHas30YearTermRuleCommand(Param.IsAny<int>(), false);
            handler = new CheckApplicationHas30YearTermRuleCommandHandler(commandHandler, appRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(IApplication));
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}
