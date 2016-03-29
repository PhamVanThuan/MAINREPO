using System;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Queries;
using CombGuid = SAHL.Core.Identity.CombGuid;
namespace SAHL.Services.WorkflowTask.Server.QueryHandlers
{
    public class GetNewTagIdQueryHandler : IServiceQueryHandler<GetNewTagIdQuery>
    {
        public ISystemMessageCollection HandleQuery(GetNewTagIdQuery query)
        {
            var newId = CombGuid.Instance.Generate();
            query.Result = new ServiceQueryResult<GetNewTagIdQueryResult>(new []{ new GetNewTagIdQueryResult { TagID = newId } });
            return SystemMessageCollection.Empty();
        }
    }
}