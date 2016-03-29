using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.CheckUnderDebtCounsellingRuleCommandHandlerSpecs
{
    [Subject(typeof(CheckUnderDebtCounsellingRuleCommandHandler))]
    public class When_executed : RuleDomainServiceSpec<CheckUnderDebtCounsellingRuleCommand, CheckUnderDebtCounsellingRuleCommandHandler>
    {
        private static string ruleName = "LegalEntityUnderDebtCounselling";
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            ILegalEntity legalEntity = An<ILegalEntity>();
            IExternalRole externalRole = An<IExternalRole>();
            externalRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

            IReadOnlyEventList<IExternalRole> externalRoles = new StubReadOnlyEventList<IExternalRole>(new IExternalRole[] { externalRole });

            IApplicationUnsecuredLending application = An<IApplicationUnsecuredLending>();
            application.WhenToldTo(x => x.ActiveClientRoles).Return(externalRoles);

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            command = new CheckUnderDebtCounsellingRuleCommand(1, false);
            handler = new CheckUnderDebtCounsellingRuleCommandHandler(commandHandler, applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters = () =>
        {
            command.RuleParameters[0].ShouldBeOfType(typeof(ILegalEntity));
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleName.ShouldBeEqualIgnoringCase(ruleName);
        };
    }
}