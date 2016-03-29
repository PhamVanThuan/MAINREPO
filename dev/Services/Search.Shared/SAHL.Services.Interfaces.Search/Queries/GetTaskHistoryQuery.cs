using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class GetTaskHistoryQuery : ServiceQuery<GetTaskHistoryQueryResult>, ISearchServiceQuery, ISqlServiceQuery<GetTaskHistoryQueryResult>
    {
        public GetTaskHistoryQuery(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public long InstanceId { get; protected set; }
    }
}