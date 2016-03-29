using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model
{
    public class GetActiveUsersWithCapabilityQueryResult
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int UserOrganisationStructureKey { get; set; }
    }
}
