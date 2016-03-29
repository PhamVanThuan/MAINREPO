using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements
{
    public class GetAttorneyInvoicesPaidThisMonthStatement : ISqlStatement<AttorneyInvoicesPaidThisMonthDataModel>
    {

        public GetAttorneyInvoicesPaidThisMonthStatement()
        {
        }

        public string GetStatement()
        {
            return @"SELECT [Count]
                        ,[Value]
                    FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth]";
        }
    }
}