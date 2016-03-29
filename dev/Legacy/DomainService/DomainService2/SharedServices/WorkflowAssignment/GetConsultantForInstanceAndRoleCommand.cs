namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetConsultantForInstanceAndRoleCommand : StandardDomainServiceCommand
    {
        public GetConsultantForInstanceAndRoleCommand(long instanceID, string dynamicRole)
        {
            this.InstanceID = instanceID;
            this.DynamicRole = dynamicRole;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public string DynamicRole
        {
            get;
            protected set;
        }

        public string Result
        {
            get;
            set;
        }
    }
}