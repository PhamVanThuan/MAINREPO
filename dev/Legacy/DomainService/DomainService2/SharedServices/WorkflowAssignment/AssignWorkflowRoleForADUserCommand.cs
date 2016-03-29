namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignWorkflowRoleForADUserCommand : StandardDomainServiceCommand
    {
        public AssignWorkflowRoleForADUserCommand(long instanceID, string adUserName, SAHL.Common.Globals.WorkflowRoleTypes workflowRoleType, int genericKey, string state)
        {
            this.InstanceID = instanceID;
            this.AdUserName = adUserName;
            this.WorkflowRoleType = workflowRoleType;
            this.GenericKey = genericKey;
            this.State = state;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public string AdUserName
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.WorkflowRoleTypes WorkflowRoleType
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }
    }
}