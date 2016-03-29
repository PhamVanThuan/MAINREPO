using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Managers.Statements;

namespace SAHL.Shared.BusinessModel.Specs.Managers.TransactionDataManagerSpecs
{
    public class when_checking_if_financial_transaction_key_exists : WithFakes
    {
        private static IDbFactory dbFactory;
        private static ITransactionDataManager transactionDataManager;
        private static int financialTransactionKey;
        private static bool result;

        Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            financialTransactionKey = 14008;
            transactionDataManager = new TransactionDataManager(dbFactory);

            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x =>
                x.SelectOne(Param<DoesFinancialTransactionKeyExistStatement>.Matches(y => y.FinancialTransactionKey == financialTransactionKey))
                ).Return(1);
        };

        Because of = () =>
        {
            result = transactionDataManager.DoesFinancialTransactionKeyExist(financialTransactionKey);
        };

        It should_check_the_db_for_the_key = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x =>
                x.SelectOne(Param<DoesFinancialTransactionKeyExistStatement>.Matches(y => y.FinancialTransactionKey == financialTransactionKey))
                );
        };

        It should_confirm_that_the_key_exists = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
