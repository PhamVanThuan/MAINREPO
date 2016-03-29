using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetDecisionTreeByNameQuery : ServiceQuery<GetDecisionTreeByNameQueryResult>, ISqlServiceQuery<GetDecisionTreeByNameQueryResult>
    {
        public GetDecisionTreeByNameQuery(string decisionTreeName)
        {
            this.DecisionTreeName = decisionTreeName;
        }

        public string DecisionTreeName { get; protected set; }
    }
}