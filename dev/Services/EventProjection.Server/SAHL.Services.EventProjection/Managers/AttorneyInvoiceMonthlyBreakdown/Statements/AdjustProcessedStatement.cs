using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class AdjustProcessedStatement : ISqlStatement<object>
    {
        public Guid AttorneyId { get; protected set; }

        public int ValueToAdd { get; protected set; }

        public AdjustProcessedStatement(Guid attorneyId, int valueToAdd)
        {
            this.AttorneyId = attorneyId;
            this.ValueToAdd = valueToAdd;
        }

        public string GetStatement()
        {
            return @"UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] SET
                        [Processed] = Processed + @ValueToAdd
                        WHERE AttorneyId = @AttorneyId";
        }
    }
}