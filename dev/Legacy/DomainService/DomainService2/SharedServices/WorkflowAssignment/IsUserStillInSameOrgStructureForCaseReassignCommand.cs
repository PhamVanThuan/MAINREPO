namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class IsUserStillInSameOrgStructureForCaseReassignCommand : StandardDomainServiceCommand
    {
        public IsUserStillInSameOrgStructureForCaseReassignCommand(int adUserKey, string dynamicRole, long instanceID)
        {
            this.ADUserKey = adUserKey;
            this.DynamicRole = dynamicRole;
            this.InstanceID = instanceID;
        }

        public int ADUserKey
        {
            get;
            protected set;
        }

        public string DynamicRole
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