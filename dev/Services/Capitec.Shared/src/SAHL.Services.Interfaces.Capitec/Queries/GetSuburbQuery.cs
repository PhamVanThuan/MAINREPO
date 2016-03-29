﻿using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.Capitec.Models;
using System;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetSuburbQuery : ServiceQuery<GetSuburbQueryResult>, ISqlServiceQuery<GetSuburbQueryResult>
    {
        public GetSuburbQuery(Guid id)
            : base(id)
        {
        }
    }
}