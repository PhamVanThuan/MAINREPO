using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Rules
{
    public class UserOrganisationStructureKeyShouldBelongToActiveADUserRule : IDomainRule<UserHasCapabilityRuleModel>
    {
        private readonly IWorkflowCaseDataManager dataManager;

        public UserOrganisationStructureKeyShouldBelongToActiveADUserRule(IWorkflowCaseDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, UserHasCapabilityRuleModel ruleModel)
        {
            var adUser = dataManager.GetADUserByUserOrganisationStructureKey(ruleModel.UserOrganisationStructureKey);
            if (adUser.GeneralStatusKey != (int)GeneralStatus.Active)
            {
                messages.AddMessage(new SystemMessage("The user organisation structure is linked to an inactive ADUser.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}