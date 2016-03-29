using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model
{
    public class GetCurrentlyAssignedUserForInstanceQueryResult
    {
        public long InstanceId { get; set; }
        public int CapabilityKey { get; set; }
        public int UserOrganisationStructureKey { get; set; }
        public string UserName { get; set; }
        public DateTime LastUpdated { get; set; }
        public string FullName { get; set; }
    }
}
