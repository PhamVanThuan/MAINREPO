using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowAssignmentDomain.Managers;

namespace SAHL.Services.WorkflowAssignmentDomain.Rules
{
    public class UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule : IDomainRule<ReturnInstanceToLastUserInCapabilityCommand>
    {
        private readonly IWorkflowCaseDataManager dataManager;

        public UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule(IWorkflowCaseDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ReturnInstanceToLastUserInCapabilityCommand ruleModel)
        {
            var lastUserInCapability = dataManager.GetLastUserAssignedInCapability((int)ruleModel.Capability, ruleModel.InstanceId);
            if (lastUserInCapability == null)
            {
                messages.AddMessage(new SystemMessage(string.Format("The case could not be returned to role: {0}. A user with that role has not previously worked on this case.",
                    ruleModel.Capability.ToString()), SystemMessageSeverityEnum.Error));
            }
        }
    }
}