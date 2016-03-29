using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Statements;

namespace SAHL.Services.FinanceDomain.Specs.Managers.FinanceDataManagerSpec
{
    public class when_retrieving_financial_transaction_key : WithFakes
    {
        private static IFinanceDataManager financeDataManager;
        private static IDbFactory dbFactory;
        private static int result, expectedResult;
        private static string reference;

        Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            reference = "someReference";
            financeDataManager = new FinanceDataManager(dbFactory);
            expectedResult = 105;

            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<GetFinancialTransactionKeyByReferenceStatement>.Matches(y => y.Reference == reference))).Return(expectedResult);
        };

        Because of = () =>
        {
            result = financeDataManager.GetFinancialTransactionKeyByReference(reference);
        };

        It should_retrieve_the_financial_transaction_key_from_the_db = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<GetFinancialTransactionKeyByReferenceStatement>.Matches(y => y.Reference == reference)));
        };

        It should_return_the_financial_transaction_key = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}
