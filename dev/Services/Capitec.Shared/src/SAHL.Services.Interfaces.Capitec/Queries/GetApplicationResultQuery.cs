using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetApplicationResultQuery : ServiceQuery<GetApplicationResultQueryResult>
    {
        public Guid ApplicationID { get; set; }

        public new GetApplicationResultQueryResult Result { get; set; }

        public GetApplicationResultQuery(Guid applicationID)
        {
            this.ApplicationID = applicationID;
        }
    }
}