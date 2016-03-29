﻿using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetLatestDecisionTreesUnpubOnlyQuery : ServiceQuery<GetLatestDecisionTreesUnpubOnlyQueryResult>, ISqlServiceQuery<GetLatestDecisionTreesUnpubOnlyQueryResult>
    {
    }
}
