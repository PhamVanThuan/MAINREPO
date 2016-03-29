using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesPaidThisYearDataManagerSpecs
{
    public class when_clearing_the_count_and_value_cols : WithFakes
    {
        private static AttorneyInvoicesPaidThisYearDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesPaidThisYearDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.ClearAttorneyInvoicesPaidThisYear();
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param.IsAny<ClearAttorneyInvoicesPaidThisYearStatement>()));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}