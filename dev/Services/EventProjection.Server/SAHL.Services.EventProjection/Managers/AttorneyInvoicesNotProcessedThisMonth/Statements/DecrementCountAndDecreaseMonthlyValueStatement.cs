using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements
{
    public class DecrementCountAndDecreaseMonthlyValueStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }
        public int ValueToDecrementBy { get; protected set; }
        public DecrementCountAndDecreaseMonthlyValueStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
            this.ValueToDecrementBy = 1;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth
                SET [Count] = [Count] - @ValueToDecrementBy, [Value] = [Value] - @InvoiceValue";
        }
    }
}