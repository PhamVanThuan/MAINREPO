namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class AssignWorkflowRoleCommand : StandardDomainServiceCommand
    {
        public AssignWorkflowRoleCommand(long instanceID, int adUserKey, int offerRoleTypeOrganisationStructureMappingKey, string stateName)
        {
            this.InstanceID = instanceID;
            this.AdUserKey = adUserKey;
            this.OfferRoleTypeOrganisationStructureMappingKey = offerRoleTypeOrganisationStructureMappingKey;
            this.StateName = stateName;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int AdUserKey
        {
            get;
            protected set;
        }

        public int OfferRoleTypeOrganisationStructureMappingKey
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