using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetBranchQuery : ServiceQuery<GetBranchQueryResult>, ISqlServiceQuery<GetBranchQueryResult>
    {
        public GetBranchQuery(Guid id)
            : base(id)
        {
        }
    }
}