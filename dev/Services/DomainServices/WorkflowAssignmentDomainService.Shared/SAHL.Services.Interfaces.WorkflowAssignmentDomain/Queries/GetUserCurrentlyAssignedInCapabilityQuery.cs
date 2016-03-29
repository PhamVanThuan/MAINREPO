using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries
{
    public class GetUserCurrentlyAssignedInCapabilityQuery : ServiceQuery<GetUserCurrentlyAssignedInCapabilityQueryResult>, IWorkflowAssignmentDomainQuery, ISqlServiceQuery<GetUserCurrentlyAssignedInCapabilityQueryResult>
    {
        public long InstanceId { get; protected set; }
        public int Capability { get; protected set; }

        public GetUserCurrentlyAssignedInCapabilityQuery(long instanceId, int capability)
        {
            this.InstanceId = instanceId;
            this.Capability = capability;
        }
    }
}