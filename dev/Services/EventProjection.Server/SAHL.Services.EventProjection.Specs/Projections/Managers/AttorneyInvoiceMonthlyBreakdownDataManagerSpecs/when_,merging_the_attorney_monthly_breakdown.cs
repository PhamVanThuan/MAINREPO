using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    internal class when__merging_the_attorney_monthly_breakdown : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid thirdPartyId;
        private static string registeredName;

        private Establish context = () =>
        {
            thirdPartyId = Guid.NewGuid();
            registeredName = "Randles (Inc)";
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.MergeAttorneyMonthlyBreakdownRecordForAttorney(thirdPartyId, registeredName);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<MergeAttorneyMonthlyBreakdownRecordForAttorneyStatement>
                .Matches(y => y.AttorneyId == thirdPartyId && y.AttorneyName == registeredName)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}