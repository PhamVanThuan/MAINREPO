using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoicesNotProcessedThisMonthDataManagerSpecs
{
    public class when_getting_value_column : WithFakes
    {
        private static AttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AttorneyInvoicesNotProcessedThisMonthDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.GetAttorneyInvoicesNotProcessedThisMonth();
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param.IsAny<GetAttorneyInvoicesNotProcessedThisMonthStatement>()));
        };
    }
}