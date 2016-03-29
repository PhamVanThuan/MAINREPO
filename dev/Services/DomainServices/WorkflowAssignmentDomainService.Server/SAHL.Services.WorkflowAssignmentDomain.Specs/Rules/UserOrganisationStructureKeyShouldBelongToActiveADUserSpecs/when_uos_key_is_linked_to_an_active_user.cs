using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.Rules.UserOrganisationStructureKeyShouldBelongToActiveADUserSpecs
{
    public class when_uos_key_is_linked_to_an_active_user : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static UserOrganisationStructureKeyShouldBelongToActiveADUserRule rule;
        private static ISystemMessageCollection messages;
        private static int userOrgStructureKey;
        private static ADUserDataModel adUser;

        private Establish that = () =>
        {
            adUser = new ADUserDataModel(1, @"SAHL\TestUser", (int)GeneralStatus.Active, "password", "passwordQuestion", "passwordAnswer", 1234567);
            userOrgStructureKey = 1;
            dataManager = An<IWorkflowCaseDataManager>();
            dataManager.WhenToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey))
                .Return(adUser);
            rule = new UserOrganisationStructureKeyShouldBelongToActiveADUserRule(dataManager);
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, new UserHasCapabilityRuleModel(userOrgStructureKey, 1));
        };

        private It should_retrieve_the_adUser_using_the_correct_uosKey = () =>
        {
            dataManager.WasToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}