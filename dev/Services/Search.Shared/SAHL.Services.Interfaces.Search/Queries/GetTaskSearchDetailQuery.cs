using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;

namespace SAHL.Services.Interfaces.Search.Queries
{
    public class GetTaskSearchDetailQuery : ServiceQuery<GetTaskSearchDetailQueryResult>, ISearchServiceQuery, ISqlServiceQuery<GetTaskSearchDetailQueryResult>
    {
        public GetTaskSearchDetailQuery(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public long InstanceId { get; protected set; }
    }
}