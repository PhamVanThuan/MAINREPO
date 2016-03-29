namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeysByProcessCommand : StandardDomainServiceCommand
    {
        public ReactiveUserOrRoundRobinForOSKeysByProcessCommand(string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey)
        {
            this.DynamicRole = dynamicRole;
            this.GenericKey = genericKey;
            this.OrganisationStructureKeys = organisationStructureKeys;
            this.InstanceID = instanceID;
            this.State = state;
            this.Process = process;
            this.RoundRobinPointerKey = roundRobinPointerKey;
        }
        public string Result
        {
            get;
            set;
        }
        public string DynamicRole
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public System.Collections.Generic.List<int> OrganisationStructureKeys
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

        public SAHL.Common.Globals.Process Process
        {
            get;
            protected set;
        }

        public int RoundRobinPointerKey
        {
            get;
            protected set;
        }
    }
}