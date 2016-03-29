namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class RoundRobinAndAssignOtherFLCasesCommand : StandardDomainServiceCommand
    {
        public RoundRobinAndAssignOtherFLCasesCommand(int applicationKey, string dynamicRole, int orgStructureKey, long instanceID, string state, int roundRobinPointerKey)
        {
            this.ApplicationKey = applicationKey;
            this.DynamicRole = dynamicRole;
            this.OrgStructureKey = orgStructureKey;
            this.InstanceID = instanceID;
            this.State = state;
            this.RoundRobinPointerKey = roundRobinPointerKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string DynamicRole
        {
            get;
            protected set;
        }

        public int OrgStructureKey
        {
            get;
            protected set;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }

        public int RoundRobinPointerKey
        {
            get;
            protected set;
        }

        public string AssignedUserResult
        {
            get;
            set;
        }
    }
}