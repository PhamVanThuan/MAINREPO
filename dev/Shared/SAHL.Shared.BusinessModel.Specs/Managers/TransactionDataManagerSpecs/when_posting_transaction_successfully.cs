using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Managers.Statements;
using SAHL.Shared.BusinessModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Shared.BusinessModel.Specs.Managers.TransactionDataManagerSpecs
{
    public class when_posting_transaction_successfully : WithFakes
    {
        private static IDbFactory dbFactory;
        private static ITransactionDataManager transactionDataManager;
        private static PostTransactionModel transactionModel;

        private Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            transactionDataManager = new TransactionDataManager(dbFactory);
            transactionModel = new PostTransactionModel(123, 480, 100m, DateTime.Now.AddDays(-2), "tra1", "system");
        };

        private Because of = () =>
        {
            transactionDataManager.PostTransaction(transactionModel);
        };

        private It should_post_transaction = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().
                WasToldTo(x => x.SelectOne<string>(Param.IsAny<PostTransactionStatement>()));
        };
    }
}
