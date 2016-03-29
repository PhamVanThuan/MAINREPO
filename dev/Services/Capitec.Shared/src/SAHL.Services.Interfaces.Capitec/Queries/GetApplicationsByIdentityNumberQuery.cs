using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetApplicationByIdentityNumberQuery : ServiceQuery<GetApplicationByIdentityNumberQueryResult>, ISqlServiceQuery<GetApplicationByIdentityNumberQueryResult>
    {
        public GetApplicationByIdentityNumberQuery(string identityNumber, string statusTypeName)
        {
            this.IdentityNumber = identityNumber;
            this.StatusTypeName = statusTypeName;
        }

        public string IdentityNumber { get; protected set; }
        public string StatusTypeName { get; protected set; }
    }
}
