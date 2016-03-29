using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetDecisionTreeQuery : ServiceQuery<GetDecisionTreeQueryResult>, ISqlServiceQuery<GetDecisionTreeQueryResult>
    {
        public GetDecisionTreeQuery(Guid decisionTreeVersionId)
        {
            this.DecisionTreeVersionId = decisionTreeVersionId;
        }

        public Guid DecisionTreeVersionId { get; protected set; }
    }
}