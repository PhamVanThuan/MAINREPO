using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_getting_a_distinct_count : WithFakes
    {
        private static AccountsPaidForAttorneyInvoicesDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static int count;
        private static int result;
        private static Guid attorneyId;

        private Establish context = () =>
        {
            attorneyId = Guid.NewGuid();
            count = 112;
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetDistinctCountOfAccountsForAttorneyStatement>())).Return(count);
            dataManager = new AccountsPaidForAttorneyInvoicesDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetDistinctCountOfAccountsForAttorney(attorneyId);
        };

        private It should_use_the_correct_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetDistinctCountOfAccountsForAttorneyStatement>
                .Matches(y => y.AttorneyId == attorneyId)));
        };

        private It should_return_the_result_of_the_statement = () =>
        {
            result.ShouldEqual(count);
        };
    }
}