using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetWorkflowAssignmentQuery : ServiceQuery<GetWorkflowAssignmentQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetWorkflowAssignmentQueryResult>
    {
        public int AppKey { get; protected set; }

        public GetWorkflowAssignmentQuery(int appKey)
        {
            this.AppKey = appKey;
        }
    }
}