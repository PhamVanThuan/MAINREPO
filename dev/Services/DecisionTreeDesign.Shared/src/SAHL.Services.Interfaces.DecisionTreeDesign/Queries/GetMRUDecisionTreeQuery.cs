using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetMRUDecisionTreeQuery : ServiceQuery<GetMRUDecisionTreeQueryResult>, ISqlServiceQuery<GetMRUDecisionTreeQueryResult>
    {
        public GetMRUDecisionTreeQuery(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; protected set; }
    }
}