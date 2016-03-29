using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class IncrementRejectedCountStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }

        public IncrementRejectedCountStatement(Guid attorneyId)
        {
            this.AttorneyId = attorneyId;
        }

        public string GetStatement()
        {
            return @"UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] SET [Rejected] = Rejected + 1 WHERE AttorneyId = @AttorneyId";
        }
    }
}