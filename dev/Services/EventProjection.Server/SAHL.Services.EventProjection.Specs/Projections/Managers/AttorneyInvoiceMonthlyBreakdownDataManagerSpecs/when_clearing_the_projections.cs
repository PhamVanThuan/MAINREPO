using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    internal class when_clearing_the_projections : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
         {
             fakeDbFactory = new FakeDbFactory();
             dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
         };

        private Because of = () =>
         {
             dataManager.ClearAttorneyInvoiceMonthlyBreakdownManagerTable();
         };

        private It should_clear_the_projection_table = () =>
         {
             fakeDbFactory.FakedDb.InAppContext().WasToldTo(x =>
                    x.ExecuteNonQuery(Param.IsAny<ClearAttorneyInvoiceMonthlyBreakdownManagerTableStatement>()));
         };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}