using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements
{
    public class IncrementCountAndIncreaseMonthlyValueStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }
        public int ValueToIncrementBy { get; protected set; }
        public IncrementCountAndIncreaseMonthlyValueStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
            this.ValueToIncrementBy = 1;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth
                SET [Count] = [Count] + @ValueToIncrementBy, [Value] = [Value] + @InvoiceValue";
        }
    }
}