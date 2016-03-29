namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommand : StandardDomainServiceCommand
    {
        public ReactivateUserOrRoundRobinForWorkflowRoleAssignmentCommand(SAHL.Common.Globals.GenericKeyTypes genericKeyType, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, SAHL.Common.Globals.RoundRobinPointers roundRobinPointer)
        {
            this.GenericKeyType = genericKeyType;
            this.WorkflowRoleType = workflowRoleType;
            this.GenericKey = genericKey;
            this.InstanceID = instanceID;
            this.RoundRobinPointer = roundRobinPointer;
        }

        public string AssignedUserResult { get; set; }

        public SAHL.Common.Globals.GenericKeyTypes GenericKeyType { get; set; }

        public SAHL.Common.Globals.WorkflowRoleTypes WorkflowRoleType { get; set; }

        public int GenericKey { get; set; }

        public long InstanceID { get; set; }

        public SAHL.Common.Globals.RoundRobinPointers RoundRobinPointer { get; set; }
    }
}