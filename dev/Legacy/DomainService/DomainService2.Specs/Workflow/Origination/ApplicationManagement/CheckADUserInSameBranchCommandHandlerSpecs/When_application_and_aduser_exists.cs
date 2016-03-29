using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CheckADUserInSameBranchCommandHandlerSpecs
{
    [Subject(typeof(CheckADUserInSameBranchRulesCommandHandler))]
    public class When_application_and_aduser_exists : RuleSetDomainServiceSpec<CheckADUserInSameBranchRulesCommand, CheckADUserInSameBranchRulesCommandHandler>
    {
        private const string ruleSet = "CheckUserOrganisationalStructure";

        Establish context = () =>
        {
            IApplication application = An<IApplication>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            IADUser user = An<IADUser>();
            IOrganisationStructureRepository orgStructureRepository = An<IOrganisationStructureRepository>();
            orgStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return(user);

            command = new CheckADUserInSameBranchRulesCommand(1, "HaloUser", true);
            handler = new CheckADUserInSameBranchRulesCommandHandler(commandHandler, orgStructureRepository, applicationRepository);
            messages = new DomainMessageCollection();
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_application_rule_parameter = () =>
        {
            command.RuleParameters[0].ShouldNotBeNull();
        };

        It should_set_aduser_rule_parameter = () =>
        {
            command.RuleParameters[1].ShouldNotBeNull();
        };

        It should_set_workflow_ruleset = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(ruleSet);
        };
    }
}