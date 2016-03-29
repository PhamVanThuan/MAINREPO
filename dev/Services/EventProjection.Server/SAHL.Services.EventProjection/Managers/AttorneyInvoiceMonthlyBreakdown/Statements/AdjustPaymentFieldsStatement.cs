using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class AdjustPaymentFieldsStatement : ISqlStatement<object>
    {
        public decimal Capitalised { get; protected set; }
        public decimal DebtReview { get; protected set; }
        public decimal PaidBySPV { get; protected set; }
        public Guid AttorneyId { get; protected set; }

        public AdjustPaymentFieldsStatement(decimal capitalised, decimal debtReview, decimal paidBySPV, Guid attorneyId)
        {
            this.Capitalised = capitalised;
            this.DebtReview = debtReview;
            this.PaidBySPV = paidBySPV;
            this.AttorneyId = attorneyId;
        }

        public string GetStatement()
        {
            return @"UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown]
                        SET [Capitalised] = Capitalised + @Capitalised, [PaidBySPV] = PaidBySPV + @PaidBySPV, [DebtReview] = DebtReview + @DebtReview
                        WHERE AttorneyId = @AttorneyId";
    }
    }
}