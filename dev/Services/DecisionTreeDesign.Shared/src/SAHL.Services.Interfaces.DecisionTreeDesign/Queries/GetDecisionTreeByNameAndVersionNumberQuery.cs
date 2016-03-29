using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetDecisionTreeByNameAndVersionNumberQuery: ServiceQuery<GetDecisionTreeByNameAndVersionNumberQueryResult>, ISqlServiceQuery<GetDecisionTreeByNameAndVersionNumberQueryResult>
    {
        public string DecisionTreeName { get; protected set; }
        public int VersionNumber { get; protected set; }

        public GetDecisionTreeByNameAndVersionNumberQuery(string decisionTreeName, int versionNumber)
        {
            this.DecisionTreeName = decisionTreeName;
            this.VersionNumber = versionNumber;
        }
    }
}
