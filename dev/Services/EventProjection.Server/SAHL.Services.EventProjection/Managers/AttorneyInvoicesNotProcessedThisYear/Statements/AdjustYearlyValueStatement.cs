using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear.Statements
{
    public class AdjustYearlyValueStatement : ISqlStatement<object>
    {
        public decimal Adjustment { get; protected set; }

        public AdjustYearlyValueStatement(decimal adjustment)
        {
            this.Adjustment = adjustment;
        }

        public string GetStatement()
        {
            return @"IF NOT EXISTS (SELECT 1 FROM EventProjection.projection.AttorneyInvoicesNotProcessedThisYear)
                BEGIN
	                INSERT INTO EventProjection.projection.AttorneyInvoicesNotProcessedThisYear ([Count], [Value]) VALUES (0,0)
                END

                UPDATE EventProjection.projection.AttorneyInvoicesNotProcessedThisYear
                SET [Value] = [Value] + @Adjustment";
        }
    }
}