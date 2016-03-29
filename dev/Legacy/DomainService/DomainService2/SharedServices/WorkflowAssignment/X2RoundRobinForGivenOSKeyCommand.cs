namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class X2RoundRobinForGivenOSKeyCommand : StandardDomainServiceCommand
    {
        public X2RoundRobinForGivenOSKeyCommand(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName)
        {
            this.DynamicRole = dynamicRole;
            this.GenericKey = genericKey;
            this.OrganisationStructureKey = organisationStructureKey;
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

        public int OrganisationStructureKey
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

        public string Result
        {
            get;
            protected set;
        }
    }
}