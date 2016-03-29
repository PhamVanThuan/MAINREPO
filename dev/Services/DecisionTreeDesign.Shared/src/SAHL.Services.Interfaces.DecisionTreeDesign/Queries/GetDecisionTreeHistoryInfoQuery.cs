using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetDecisionTreeHistoryInfoQuery : ServiceQuery<GetDecisionTreeHistoryInfoQueryResult>, ISqlServiceQuery<GetDecisionTreeHistoryInfoQueryResult>
    {
        public Guid DecisionTreeId { get; protected set; }

        public GetDecisionTreeHistoryInfoQuery(Guid decisionTreeId)
        {
            this.DecisionTreeId = decisionTreeId;
        }
    }
}