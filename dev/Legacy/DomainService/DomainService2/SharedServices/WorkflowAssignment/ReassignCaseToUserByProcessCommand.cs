namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReassignCaseToUserByProcessCommand : StandardDomainServiceCommand
    {
        public ReassignCaseToUserByProcessCommand(long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName, SAHL.Common.Globals.Process processName)
        {
            this.InstanceID = instanceID;
            this.GenericKey = genericKey;
            this.AdUserName = adUserName;
            this.OrganisationStructureKey = organisationStructureKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.StateName = stateName;
            this.ProcessName = processName;
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

        public SAHL.Common.Globals.Process ProcessName { get; set; }
    }
}