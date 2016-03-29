namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class CloneActiveSecurityFromInstanceForInstanceCommand : StandardDomainServiceCommand
    {
        public CloneActiveSecurityFromInstanceForInstanceCommand(long parentInstanceID, long instanceID)
        {
            this.ParentInstanceID = parentInstanceID;
            this.InstanceID = instanceID;
        }

        public long ParentInstanceID
        {
            get;
            protected set;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}