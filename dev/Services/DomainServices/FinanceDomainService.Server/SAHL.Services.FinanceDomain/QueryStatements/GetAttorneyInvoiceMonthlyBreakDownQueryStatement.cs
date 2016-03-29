using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.QueryStatements
{
    public class GetAttorneyInvoiceMonthlyBreakDownQueryStatement :
        IServiceQuerySqlStatement<GetAttorneyInvoiceMonthlyBreakDownQuery, AttorneyInvoiceMonthlyBreakdownDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT [AttorneyId]
                      ,[AttorneyName]
                      ,[Capitalised]
                      ,[PaidBySPV]
                      ,[DebtReview]
                      ,[Total]
                      ,[AvgRValuePerInvoice]
                      ,[AvgRValuePerAccount]
                      ,[Paid]
                      ,[Rejected]
                      ,[Unprocessed]
                      ,[Processed]
                      ,[AccountsPaid]
                  FROM [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown]
                  Order by [AttorneyName]";
        }
    }
}