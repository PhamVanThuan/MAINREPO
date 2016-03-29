namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class CheckUserInWorkflowRoleCommand : StandardDomainServiceCommand
    {
        public CheckUserInWorkflowRoleCommand(string aDUserName, int workflowRoleTypeKey)
        {
            this.ADUserName = aDUserName;
            this.WorkflowRoleTypeKey = workflowRoleTypeKey;
        }

        public string ADUserName
        {
            get;
            protected set;
        }

        public int WorkflowRoleTypeKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}