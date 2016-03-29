namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStructCommand : StandardDomainServiceCommand
    {
        public GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStructCommand(long sourceInstanceID, string dynamicRole, int organisationStructureKey)
        {
            this.SourceInstanceID = sourceInstanceID;
            this.DynamicRole = dynamicRole;
            this.OrganisationStructureKey = organisationStructureKey;
        }

        public long SourceInstanceID
        {
            get;
            protected set;
        }

        public string DynamicRole
        {
            get;
            protected set;
        }

        public int OrganisationStructureKey
        {
            get;
            protected set;
        }

        public string AssignedUser
        {
            get;
            set;
        }
    }
}