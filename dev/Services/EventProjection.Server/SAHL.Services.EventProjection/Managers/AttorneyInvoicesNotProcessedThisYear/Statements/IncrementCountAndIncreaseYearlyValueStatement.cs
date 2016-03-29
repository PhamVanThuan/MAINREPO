using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear.Statements
{
    public class IncrementCountAndIncreaseYearlyValueStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }
        public int ValueToIncrementBy { get; protected set; }
        public IncrementCountAndIncreaseYearlyValueStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
            this.ValueToIncrementBy = 1;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisYear)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisYear ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisYear
                SET [Count] = [Count] + @ValueToIncrementBy, [Value] = [Value] + @InvoiceValue";
        }
    }
}