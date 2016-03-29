namespace DomainService2.SharedServices.Common
{
    public class CheckIfUserIsPartOfOrgStructureCommand : StandardDomainServiceCommand
    {
        public CheckIfUserIsPartOfOrgStructureCommand(SAHL.Common.Globals.OrganisationStructure organisationStructure, string adUserName)
        {
            this.OrganisationStructure = organisationStructure;
            this.ADUserName = adUserName;
        }

        public SAHL.Common.Globals.OrganisationStructure OrganisationStructure
        {
            get;
            protected set;
        }

        public string ADUserName
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}