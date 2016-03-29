
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AccountsPaidForAttorneyInvoicesDataManagerSpecs
{
    public class when_clearing_the_monthly_data : WithFakes
    {
        private static AccountsPaidForAttorneyInvoicesDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AccountsPaidForAttorneyInvoicesDataManager(fakeDbFactory);
        };

        Because of = () =>
        {
            dataManager.ClearAccountsPaidForAttorneyInvoicesMonthly();
        };

        It should_delete_the_monthly_data = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(x => x.Delete(Param.IsAny<ClearAccountsPaidForAttorneyInvoicesMonthlyStatement>()));
        };

        It should_complete_the_transaction = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete()).OnlyOnce();
        };
    }
}
