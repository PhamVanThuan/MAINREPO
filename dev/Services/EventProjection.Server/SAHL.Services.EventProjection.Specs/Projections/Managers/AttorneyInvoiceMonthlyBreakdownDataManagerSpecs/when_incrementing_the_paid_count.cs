using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_incrementing_the_paid_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;

        private Establish context = () =>
        {
            thirdPartyId = Guid.NewGuid();
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.IncrementPaidCount(thirdPartyId);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<IncrementPaidCountStatement>
                .Matches(y => y.AttorneyId == thirdPartyId)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}