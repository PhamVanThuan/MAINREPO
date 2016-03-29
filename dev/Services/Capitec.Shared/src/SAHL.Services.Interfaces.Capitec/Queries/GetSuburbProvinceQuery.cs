using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetSuburbProvinceQuery : ServiceQuery<GetSuburbProvinceQueryResult>, ISqlServiceQuery<GetSuburbProvinceQueryResult>
    {
        public GetSuburbProvinceQuery(Guid id)
            : base(id)
        {
        }
    }
}