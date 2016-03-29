namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignCaseToUserCommand : StandardDomainServiceCommand
    {
        public ReassignCaseToUserCommand(long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName)
        {
            this.InstanceID = instanceID;
            this.GenericKey = genericKey;
            this.AdUserName = adUserName;
            this.OrganisationStructureKey = organisationStructureKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.StateName = stateName;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string AdUserName
        {
            get;
            protected set;
        }

        public int OrganisationStructureKey
        {
            get;
            protected set;
        }

        public int OfferRoleTypeKey
        {
            get;
            protected set;
        }

        public string StateName
        {
            get;
            protected set;
        }
    }
}