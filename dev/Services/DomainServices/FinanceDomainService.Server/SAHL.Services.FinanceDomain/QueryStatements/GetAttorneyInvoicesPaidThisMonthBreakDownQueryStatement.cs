﻿using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.QueryStatements
{
    public class GetAttorneyInvoicesPaidThisMonthBreakDownQueryStatement :
           IServiceQuerySqlStatement<GetAttorneyInvoicesPaidThisMonthBreakDownQuery, AttorneyInvoicesPaidThisMonthDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT 
                        [Count]
                        ,[Value]
                    FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth]";
        }
    }
}
