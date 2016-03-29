using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class ClearAttorneyInvoiceMonthlyBreakdownManagerTableStatement : ISqlStatement<object>
    {
        public string GetStatement()
        {
            return @"UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown]
                    SET 
                        Capitalised = 0,
                        PaidBySPV = 0,
                        DebtReview = 0,
                        Paid = 0,
                        Rejected = 0,
                        AccountsPaid = 0";
        }
    }
}
