using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetUserFromAuthTokenQuery : ServiceQuery<GetUserFromAuthTokenQueryResult>, ISqlServiceQuery<GetUserFromAuthTokenQueryResult>
    {
        public GetUserFromAuthTokenQuery(Guid authenticationToken)
        {
            this.AuthenticationToken = authenticationToken;
        }

        public Guid AuthenticationToken { get; protected set; }
    }
}