using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements
{
    public class ClearAttorneyInvoicesPaidThisMonthStatement : ISqlStatement<AttorneyInvoicesPaidThisMonthDataModel>
    {

        public ClearAttorneyInvoicesPaidThisMonthStatement()
        {
        }

        public string GetStatement()
        {
            return @"IF EXISTS (select * from [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth]) 
                        UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] SET Count = 0, Value = 0 
                     ELSE
                    	INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] ([Count], [Value]) VALUES (0, 0)";
        }
    }
}