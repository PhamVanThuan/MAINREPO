using System.Collections.Generic;

namespace SAHL.Services.Interfaces.UserProfile.Models.Shared
{
    public class OrganisationStructureResult
    {
        public List<OrganisationStructure> OrganisationalStructures { get; set; }

        public List<OrganisationStructureMapping> Mappings { get; set; }

        public List<object> Users { get; set; }
    }
}