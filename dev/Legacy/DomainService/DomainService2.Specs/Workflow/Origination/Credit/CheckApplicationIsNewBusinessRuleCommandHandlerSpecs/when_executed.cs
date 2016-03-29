using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.Workflow.Origination.Credit.CheckApplicationIsNewBusinessRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckApplicationIsNewBusinessRuleCommandHandler))]
    public class when_executed : RuleDomainServiceSpec<CheckApplicationIsNewBusinessRuleCommand, CheckApplicationIsNewBusinessRuleCommandHandler>
    {
        private static string ruleName = "ApplicationIsNewBusiness";
        private static IApplicationRepository appRepo;
        private static IApplication app;

        Establish context = () =>
        {
            appRepo = An<IApplicationRepository>();
            app = An<IApplication>();
            appRepo.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);
            command = new CheckApplicationIsNewBusinessRuleCommand(Param.IsAny<int>(), false);
            handler = new CheckApplicationIsNewBusinessRuleCommandHandler(appRepo,commandHandler);
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
