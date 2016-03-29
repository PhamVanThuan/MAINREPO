using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    internal class when_adjusting_the_processed_count : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;
        private static int valueToAdd;

        private Establish context = () =>
         {
             thirdPartyId = Guid.NewGuid();
             valueToAdd = 1;
             fakeDbFactory = new FakeDbFactory();
             dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
         };

        private Because of = () =>
         {
             dataManager.AdjustProcessedCount(thirdPartyId, valueToAdd);
         };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
         {
             fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<AdjustProcessedStatement>
                 .Matches(y => y.AttorneyId == thirdPartyId && y.ValueToAdd == valueToAdd)));
         };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}