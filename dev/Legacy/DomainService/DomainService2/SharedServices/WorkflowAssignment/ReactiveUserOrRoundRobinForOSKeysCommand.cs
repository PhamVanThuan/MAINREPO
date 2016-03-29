namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeysCommand : StandardDomainServiceCommand
    {
        public ReactiveUserOrRoundRobinForOSKeysCommand(string dynamicRole, int genericKey, System.Collections.Generic.List<int> organisationStructureKeys, long instanceID, string stateName)
        {
            this.DynamicRole = dynamicRole;
            this.GenericKey = genericKey;
            this.OrganisationStructureKeys = organisationStructureKeys;
            this.InstanceID = instanceID;
            this.StateName = stateName;
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

        public string StateName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}