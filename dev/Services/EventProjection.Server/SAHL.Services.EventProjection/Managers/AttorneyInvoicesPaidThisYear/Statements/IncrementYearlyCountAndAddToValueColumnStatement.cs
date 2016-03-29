using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear.Statements
{
    public class IncrementYearlyCountAndAddToValueColumnStatement : ISqlStatement<object>
    {
        public decimal InvoiceValue { get; protected set; }

        public IncrementYearlyCountAndAddToValueColumnStatement(decimal invoiceValue)
        {
            this.InvoiceValue = invoiceValue;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesPaidThisYear)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesPaidThisYear ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesPaidThisYear
                SET [Count] = [Count] + 1, [Value] = [Value] + @InvoiceValue";
        }
    }
}