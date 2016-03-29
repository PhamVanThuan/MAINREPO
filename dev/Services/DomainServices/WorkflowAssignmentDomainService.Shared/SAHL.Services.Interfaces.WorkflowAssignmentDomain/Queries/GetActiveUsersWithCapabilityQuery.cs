using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries
{
    public class GetActiveUsersWithCapabilityQuery : ServiceQuery<GetActiveUsersWithCapabilityQueryResult>, IWorkflowAssignmentDomainQuery, ISqlServiceQuery<GetActiveUsersWithCapabilityQueryResult>
    {
        public int CapabilityKey { get; protected set; }

        public GetActiveUsersWithCapabilityQuery(int capabilityKey)
        {
            this.CapabilityKey = capabilityKey;
        }
    }
}
