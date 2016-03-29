namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2RoundRobinForPointerDescriptionCommand : StandardDomainServiceCommand
    {
        public X2RoundRobinForPointerDescriptionCommand(long instanceID, int roundRobinPointerKey, int genericKey, string dynamicRole, string state, SAHL.Common.Globals.Process process)
        {
            this.InstanceID = instanceID;
            this.RoundRobinPointerKey = roundRobinPointerKey;
            this.GenericKey = genericKey;
            this.DynamicRole = dynamicRole;
            this.State = state;
            this.Process = process;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int RoundRobinPointerKey
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string DynamicRole
        {
            get;
            protected set;
        }

        public string State
        {
            get;
            protected set;
        }

        public SAHL.Common.Globals.Process Process
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