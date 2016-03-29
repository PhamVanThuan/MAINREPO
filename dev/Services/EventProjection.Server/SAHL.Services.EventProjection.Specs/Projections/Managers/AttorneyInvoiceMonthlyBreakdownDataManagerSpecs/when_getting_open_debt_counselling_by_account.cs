using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AttorneyInvoiceMonthlyBreakdownDataManagerSpecs
{
    public class when_getting_open_debt_counselling_by_account : WithFakes
    {
        private static AttorneyInvoiceMonthlyBreakdownDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static int accountKey;
        private static DebtCounsellingDataModel dataModel;
        private static DebtCounsellingDataModel result;

        private Establish context = () =>
        {
            dataModel = new DebtCounsellingDataModel(1, 123, 1, null, null, null, "1234");
            accountKey = 12345;
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetOpenDebtCounsellingByAccountStatement>())).Return(dataModel);
            dataManager = new AttorneyInvoiceMonthlyBreakdownDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            result = dataManager.GetOpenDebtCounsellingByAccountKey(accountKey);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetOpenDebtCounsellingByAccountStatement>
                .Matches(y => y.AccountKey == accountKey)));
        };

        private It should_return_the_result_from_the_database = () =>
        {
            result.ShouldEqual(dataModel);
        };
    }
}