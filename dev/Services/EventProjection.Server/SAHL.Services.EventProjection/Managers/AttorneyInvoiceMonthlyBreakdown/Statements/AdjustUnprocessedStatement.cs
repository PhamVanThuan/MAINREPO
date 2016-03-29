using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class AdjustUnprocessedStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }

        public string AttorneyName { get; protected set; }
        public int ValueToAdd { get; protected set; }

        public AdjustUnprocessedStatement(Guid attorneyId, int valueToAdd)
        {
            this.AttorneyId = attorneyId;
            this.ValueToAdd = valueToAdd;
        }

        public string GetStatement()
        {
            return @"UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] SET
                        [Unprocessed] = Unprocessed + @ValueToAdd
                        WHERE AttorneyId = @AttorneyId";
        }
    }
}