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
    public class GetCurrentlyAssignedUserForInstanceQuery : ServiceQuery<GetCurrentlyAssignedUserForInstanceQueryResult>, IWorkflowAssignmentDomainQuery, ISqlServiceQuery<GetCurrentlyAssignedUserForInstanceQueryResult>
    {
        public long InstanceId { get; protected set; }

        public GetCurrentlyAssignedUserForInstanceQuery(long instanceId)
        {
            this.InstanceId = instanceId;
        }
    }
}
