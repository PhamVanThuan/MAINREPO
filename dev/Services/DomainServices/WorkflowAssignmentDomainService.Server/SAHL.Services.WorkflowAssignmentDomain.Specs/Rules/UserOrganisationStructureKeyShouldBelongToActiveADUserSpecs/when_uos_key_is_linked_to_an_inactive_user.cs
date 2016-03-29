using System.Linq;
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
    public class when_uos_key_is_linked_to_an_inactive_user : WithFakes
    {
        private static IWorkflowCaseDataManager dataManager;
        private static UserOrganisationStructureKeyShouldBelongToActiveADUserRule rule;
        private static ISystemMessageCollection messages;
        private static int userOrgStructureKey;
        private static ADUserDataModel adUser;

        private Establish that = () =>
        {
            adUser = new ADUserDataModel(1, @"SAHL\TestUser", (int)GeneralStatus.Inactive, "password", "passwordQuestion", "passwordAnswer", 1234567);
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

        private It should_have_an_error_message = () =>
        {
            messages.ErrorMessages().Any(x => x.Message.Equals("The user organisation structure is linked to an inactive ADUser.")).ShouldBeTrue();
        };
    }
}