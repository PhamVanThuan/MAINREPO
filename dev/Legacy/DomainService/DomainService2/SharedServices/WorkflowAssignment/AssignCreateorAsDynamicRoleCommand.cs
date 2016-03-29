namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignCreateorAsDynamicRoleCommand : StandardDomainServiceCommand
    {
        public AssignCreateorAsDynamicRoleCommand(long instanceID, string dynamicRole, int genericKey, string stateName)
        {
            this.InstanceID = instanceID;
            this.DynamicRole = dynamicRole;
            this.GenericKey = genericKey;
            this.StateName = stateName;
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

        public string AssignedTo
        {
            get;
            set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string StateName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}