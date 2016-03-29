using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.WorkflowTask.Queries
{
    public class GetAllTagsForUserQuery : ServiceQuery<GetAllTagsForUserQueryResult>, IWorkflowTaskQuery
    {
        public GetAllTagsForUserQuery(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}