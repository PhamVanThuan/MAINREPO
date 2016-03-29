using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesPaidThisMonthDataManagerSpecs
{
    public class when_incrementing_the_count_and_value_cols : WithFakes
    {
        private static AttorneyInvoicesPaidThisMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static decimal value;

        private Establish context = () =>
        {
            value = 565M;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesPaidThisMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.IncrementCountAndAddInvoiceToValueColumn(value);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<IncrementMonthlyCountAndToValueColumnStatement>
                .Matches(y => y.InvoiceValue == value)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}