using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesNotProcessedThisMonthDataManagerSpecs
{
    public class when_incrementing_values : WithFakes
    {
        private static AttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static decimal invoiceValue;

        private Establish context = () =>
        {
            invoiceValue = 525M;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesNotProcessedThisMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.IncrementCountAndIncreaseMonthlyValue(invoiceValue);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<IncrementCountAndIncreaseMonthlyValueStatement>
                .Matches(y => y.InvoiceValue == invoiceValue && y.ValueToIncrementBy == 1)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}