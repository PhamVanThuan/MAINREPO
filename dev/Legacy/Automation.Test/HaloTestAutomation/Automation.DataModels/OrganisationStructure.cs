using Common.Enums;

namespace Automation.DataModels
{
    public sealed class OrganisationStructure
    {
        public int OrganisationStructureKey { get; set; }

        public int ParentKey { get; set; }

        public string Description { get; set; }

        public OrganisationTypeEnum OrganisationTypeKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public string OrganisationTypeDescription { get; set; }
    }
}