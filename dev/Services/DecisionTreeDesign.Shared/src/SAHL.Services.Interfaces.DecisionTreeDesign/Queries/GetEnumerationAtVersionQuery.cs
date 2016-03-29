using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using System;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Queries
{
    [AuthorisedCommand(Roles = "Domain Users")]
    public class GetEnumerationAtVersionQuery : ServiceQuery<GetEnumerationSetQueryResult>, ISqlServiceQuery<GetEnumerationSetQueryResult>
    {
        public GetEnumerationAtVersionQuery(Guid id)
            : base(id)
        {
        }
    }
}