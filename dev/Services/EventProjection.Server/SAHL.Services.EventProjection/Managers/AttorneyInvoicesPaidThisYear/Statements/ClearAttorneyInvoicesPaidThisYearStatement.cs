using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear.Statements
{
    public class ClearAttorneyInvoicesPaidThisYearStatement : ISqlStatement<AttorneyInvoicesPaidThisYearDataModel>
    {

        public ClearAttorneyInvoicesPaidThisYearStatement()
        {
        }

        public string GetStatement()
        {
            return @"IF EXISTS (select * from [EventProjection].[projection].[AttorneyInvoicesPaidThisYear]) 
                        UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] SET Count = 0, Value = 0 
                     ELSE
                    	INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] ([Count], [Value]) VALUES (0, 0)";
        }
    }
}