using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesNotProcessedLastMonthSpecs
{
    public class when_update_attorney_invoices_not_processed_last_month : WithFakes
    {
        private static AttorneyInvoicesNotProcessedThisMonthDataModel model;
        private static AttorneyInvoicesNotProcessedLastMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            model = new AttorneyInvoicesNotProcessedThisMonthDataModel(10, 10000);
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesNotProcessedLastMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.UpdateAttorneyInvoicesNotProcessedLastMonth(model);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<UpdateAttorneyInvoicesNotProcessedLastMonthStatement>
                .Matches(y => y.Count == model.Count && y.Value == model.Value)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}