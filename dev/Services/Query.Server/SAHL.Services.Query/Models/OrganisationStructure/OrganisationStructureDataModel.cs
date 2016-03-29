using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Query.Models.OrganisationStructure
{
    public class OrganisationStructureDataModel
    {
        public int OrganisationStructureKey { get; set; }
        public int ParentKey { get; set; }
        public string Description { get; set; }
        public int OrganisationTypeKey { get; set; }
        public int GeneralStatusKey { get; set; }

        public string OrganisationType { get; set; }
    }
}
