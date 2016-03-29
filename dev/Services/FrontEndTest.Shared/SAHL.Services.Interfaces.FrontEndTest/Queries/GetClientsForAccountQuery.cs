﻿using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetClientsForAccountQuery : ServiceQuery<GetClientsForAccountQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetClientsForAccountQueryResult>
    {
    }
}