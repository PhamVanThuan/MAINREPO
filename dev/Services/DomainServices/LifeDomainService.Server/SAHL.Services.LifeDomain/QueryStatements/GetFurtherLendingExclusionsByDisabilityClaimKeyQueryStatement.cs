﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LifeDomain.QueryStatements
{
    public class GetFurtherLendingExclusionsByDisabilityClaimKeyQueryStatement 
        : IServiceQuerySqlStatement<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery, DisabilityClaimFurtherLendingExclusionModel>
    {
        public string GetStatement()
        {
            return @"SELECT * FROM [Process].[life].[fGetDisabilityPaymentFurtherLendingExceptions] (@disabilityClaimKey)";
        }
    }
}
