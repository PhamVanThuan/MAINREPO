using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class IncrementPaidCountStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }

        public IncrementPaidCountStatement(Guid attorneyId)
        {
            this.AttorneyId = attorneyId;
        }

        public string GetStatement()
        {
            return @"UPDATE [eventprojection].[projection].[AttorneyInvoiceMonthlyBreakdown] SET [Paid] = Paid + 1 WHERE AttorneyId = @AttorneyId";
        }
    }
}