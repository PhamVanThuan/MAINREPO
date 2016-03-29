using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    internal class when_adjusting_the_accounts_paid_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;
        private static int value;

        private Establish context = () =>
         {
             thirdPartyId = Guid.NewGuid();
             value = 1;
             fakeDbFactory = new FakeDbFactory();
             dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
         };

        private Because of = () =>
         {
             dataManager.AdjustAccountsPaidCount(thirdPartyId, value);
         };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
         {
             fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<AdjustAccountsPaidStatement>
                 .Matches(y => y.AttorneyId == thirdPartyId && y.Value == value)));
         };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}