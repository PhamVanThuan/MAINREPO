using SAHL.Core.Services;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Search.QueryStatements
{
    public class GetTaskSearchDetailQueryStatement : IServiceQuerySqlStatement<GetTaskSearchDetailQuery, GetTaskSearchDetailQueryResult>
    {
        public string GetStatement()
        {
            return @"select i.CreationDate as WorkflowTaskAge, i.StateChangeDate as StateAge, i.ParentInstanceID as ParentTaskId, w.Name as ParentTaskWorkflowName,
                wsi.Name as SourceWorkflowName from [x2].[x2].Instance i
                left join [x2].[x2].Instance ip on ip.ID = i.ParentInstanceID
                left join [x2].[x2].workflow w on w.ID = ip.WorkFlowID
                left join [x2].[x2].Instance isi on isi.ID = i.SourceInstanceID
                left join [x2].[x2].workflow wsi on wsi.ID = isi.WorkFlowID
                where i.ID = @InstanceId";
        }
    }
}