using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesPaidLastMonthSpecs
{
    public class when_update_attorney_invoices_paid_last_month : WithFakes
    {
        private static AttorneyInvoicesPaidThisMonthDataModel model;
        private static AttorneyInvoicesPaidLastMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            model = new AttorneyInvoicesPaidThisMonthDataModel(10, 10000);
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesPaidLastMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.UpdateAttorneyInvoicesPaidLastMonth(model);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<UpdateAttorneyInvoicesPaidLastMonthStatement>
                .Matches(y => y.Count == model.Count && y.Value == model.Value)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}