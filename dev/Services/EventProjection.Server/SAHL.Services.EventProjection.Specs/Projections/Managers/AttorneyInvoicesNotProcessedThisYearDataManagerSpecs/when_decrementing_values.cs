using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesNotProcessedThisYearDataManagerSpecs
{
    public class when_decrementing_values : WithFakes
    {
        private static AttorneyInvoicesNotProcessedThisYearDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static decimal invoiceValue;

        private Establish context = () =>
        {
            invoiceValue = 525M;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesNotProcessedThisYearDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.DecrementCountAndDecreaseYearlyValue(invoiceValue);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<DecrementCountAndDecreaseYearlyValueStatement>
                .Matches(y => y.InvoiceValue == invoiceValue && y.ValueToDecrementBy == 1)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}