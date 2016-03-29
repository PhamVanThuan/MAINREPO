using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesPaidThisMonthDataManagerSpecs
{
    public class when_getting_the_count_and_value_cols : WithFakes
    {
        private static AttorneyInvoicesPaidThisMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesPaidThisMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.GetAttorneyInvoicesPaidThisMonthStatement();
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param.IsAny<GetAttorneyInvoicesPaidThisMonthStatement>()));
        };
    }
}