using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear.Statements
{
    public class DecrementCountAndDecreaseYearlyValueStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }
        public int ValueToDecrementBy { get; protected set; }
        public DecrementCountAndDecreaseYearlyValueStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
            this.ValueToDecrementBy = 1;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisYear)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisYear ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisYear
                SET [Count] = [Count] - @ValueToDecrementBy, [Value] = [Value] - @InvoiceValue";
        }
    }
}