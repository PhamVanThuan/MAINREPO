using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowTask.Models;

namespace SAHL.Services.Interfaces.WorkflowTask.Queries
{
    public class GetAllWorkflowStatesQuery : ServiceQuery<GetAllWorkflowStatesQueryResult>, ISqlServiceQuery<GetAllWorkflowStatesQueryResult>, IWorkflowTaskQuery
    {
        public GetAllWorkflowStatesQuery()
        {
        }
    }
}