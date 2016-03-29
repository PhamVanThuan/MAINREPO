using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Services.WorkflowAssignmentDomain.Rules
{
    public class UserOrganisationStructureMustHaveCapabilityRule : IDomainRule<UserHasCapabilityRuleModel>
    {
        private readonly IWorkflowCaseDataManager dataManager;

        public UserOrganisationStructureMustHaveCapabilityRule(IWorkflowCaseDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, UserHasCapabilityRuleModel ruleModel)
        {
            var userCapabilities = dataManager.GetCapabilitiesForUserOrganisationStructureKey(ruleModel.UserOrganisationStructureKey);
            if (!userCapabilities.Where(x=>x.CapabilityKey == ruleModel.CapabilityKey).Any())
            {
                messages.AddMessage(new SystemMessage("The organisation structure in which the user belongs no longer has specified capability", SystemMessageSeverityEnum.Error));
            }
        }
    }
}
