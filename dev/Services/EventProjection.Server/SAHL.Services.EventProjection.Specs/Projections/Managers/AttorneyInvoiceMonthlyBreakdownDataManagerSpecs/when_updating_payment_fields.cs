using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_updating_payment_fields : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static decimal debtReview;
        private static decimal paidBySPV;
        private static decimal capitalised;
        private static Guid attorneyId;

        private Establish context = () =>
        {
            attorneyId = Guid.NewGuid();
            debtReview = 500M;
            paidBySPV = 500M;
            capitalised = 500M;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.UpdatePaymentFieldsForAttorney(attorneyId, debtReview, paidBySPV, capitalised);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<AdjustPaymentFieldsStatement>
                .Matches(y => y.AttorneyId == attorneyId && y.Capitalised == capitalised && y.DebtReview == debtReview && y.PaidBySPV == paidBySPV)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };

    }
}