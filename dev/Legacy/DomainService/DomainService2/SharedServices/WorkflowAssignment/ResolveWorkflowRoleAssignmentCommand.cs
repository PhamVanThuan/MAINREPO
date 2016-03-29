namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ResolveWorkflowRoleAssignmentCommand : StandardDomainServiceCommand
    {
        public ResolveWorkflowRoleAssignmentCommand(long instanceID, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, SAHL.Common.Globals.WorkflowRoleTypeGroups workflowRoleTypeGroup)
        {
            this.InstanceID = instanceID;
            this.WorkflowRoleType = workflowRoleType;
            this.WorkflowRoleTypeGroup = workflowRoleTypeGroup;
        }

        public long InstanceID { get; protected set; }

        public SAHL.Common.Globals.WorkflowRoleTypes WorkflowRoleType { get; protected set; }

        public SAHL.Common.Globals.WorkflowRoleTypeGroups WorkflowRoleTypeGroup { get; protected set; }

        public string ADUserNameResult { get; set; }
    }
}