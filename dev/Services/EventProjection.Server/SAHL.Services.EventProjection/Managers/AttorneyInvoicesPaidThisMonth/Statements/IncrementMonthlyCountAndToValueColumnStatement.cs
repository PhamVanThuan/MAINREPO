using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements
{
    public class IncrementMonthlyCountAndToValueColumnStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }

        public IncrementMonthlyCountAndToValueColumnStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesPaidThisMonth)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesPaidThisMonth ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesPaidThisMonth
                SET [Count] = [Count] + 1, [Value] = [Value] + @InvoiceValue";
        }
    }
}