using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Managers.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Shared.BusinessModel.Specs.Managers.TransactionDataManagerSpecs
{
    public class when_checking_if_txn_type_exist: WithFakes
    {
        private static IDbFactory dbFactory;
        private static ITransactionDataManager transactionDataManager;
        private static bool expectedResult;
        private static bool actualResult;
        private static int transactionTypeKey;

        private Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            transactionDataManager = new TransactionDataManager(dbFactory);
            transactionTypeKey = 232;
            expectedResult = true;
            dbFactory.NewDb().InReadOnlyAppContext().
                WhenToldTo(x => x.SelectOne<int>(Param.IsAny<DoesTransactionTypeExistStatement>())).Return(1);
        };

        private Because of = () =>
        {
            actualResult = transactionDataManager.DoesTransactionTypeExist(transactionTypeKey);
        };

        private It should_query_using_account_dkey = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().
                WasToldTo(x => x.SelectOne<int>(Param.IsAny<DoesTransactionTypeExistStatement>()));
        };

        private It should_return_financial_services = () =>
        {
            actualResult.ShouldEqual(expectedResult);
        };
    }
}
