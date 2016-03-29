using System.Collections.Generic;

namespace SAHL.Services.Interfaces.UserProfile.Models.Shared
{
    public class OrganisationStructureMapping
    {
        public int OrganisationStructureId { get; set; }

        public List<int> Subordinates { get; set; }

        public List<int> UserIds { get; set; }
    }
}