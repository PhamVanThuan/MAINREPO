using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements
{
    public class AdjustMonthlyValueStatement : ISqlStatement<object>
    {
        public decimal Adjustment { get; protected set; }

        public AdjustMonthlyValueStatement(decimal adjustment)
        {
            this.Adjustment = adjustment;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisMonth
                SET [Value] = [Value] + @Adjustment";
        }
    }
}