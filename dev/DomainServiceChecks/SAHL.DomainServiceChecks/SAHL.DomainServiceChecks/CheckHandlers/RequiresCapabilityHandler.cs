using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;
using SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresCapabilityHandler : IDomainCommandCheckHandler<IRequiresCapability>
    {
        private IWorkflowAssignmentDataManager workflowAssignmentDataManager;

        public RequiresCapabilityHandler(IWorkflowAssignmentDataManager workflowAssignmentDataManager)
        {
            this.workflowAssignmentDataManager = workflowAssignmentDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresCapability command)
        {
            var messages = SystemMessageCollection.Empty();
            if (!workflowAssignmentDataManager.DoesCapabilityExist(command.CapabilityKey))
            {
                messages.AddMessage(new SystemMessage("The capability provided does not exist.", SystemMessageSeverityEnum.Error));
            }
            return messages;
        }

    }
}