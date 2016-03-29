using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_creating_an_AssignWorkflowCaseCommandHandler : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static IEventRaiser eventRaiser;
        private static IDomainRuleManager<UserHasCapabilityRuleModel> domainRuleManager;

        private Establish that = () =>
        {
            dataManager = An<IWorkflowCaseDataManager>();
            eventRaiser = An<IEventRaiser>();
            domainRuleManager = An<IDomainRuleManager<UserHasCapabilityRuleModel>>();
        };

        private Because of = () =>
        {
            new AssignWorkflowCaseCommandHandler(dataManager, eventRaiser, domainRuleManager);
        };

        private It should_have_registered_the_user_organisation_structure_must_have_capability_rules = () =>
        {
            domainRuleManager.WasToldTo(
                a => a.RegisterRule(Param<IDomainRule<UserHasCapabilityRuleModel>>.IsA<UserOrganisationStructureMustHaveCapabilityRule>()));
        };

        private It should_have_registered_the_user_organisation_structure_key_should_belong_to_active_ad_user_rules = () =>
        {
            domainRuleManager.WasToldTo(
                a => a.RegisterRule(Param<IDomainRule<UserHasCapabilityRuleModel>>.IsA<UserOrganisationStructureKeyShouldBelongToActiveADUserRule>()));
        };
    }
}