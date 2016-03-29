using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Managers.Statements;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Specs.Managers.TransactionDataManagerSpecs
{
    public class when_posting_reversal_transaction_returns_errors : WithFakes
    {
        private static IDbFactory dbFactory;
        private static ITransactionDataManager transactionDataManager;
        private static string result;
        private static ReversalTransactionModel transactionModel;

        Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            transactionDataManager = new TransactionDataManager(dbFactory);
            transactionModel = new ReversalTransactionModel(14008, "SystemUser");

            dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<PostReversalTransactionStatement>
               .Matches(y => y.FinancialTransactionKey == transactionModel.FinancialTransactionKey && y.UserID == transactionModel.UserId))
               ).Return("An error occured");

        };


        Because of = () =>
        {
            result = transactionDataManager.PostReversalTransaction(transactionModel);
        };

        It should_post_the_reversal_transaction = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Param<PostReversalTransactionStatement>
                .Matches(y => y.FinancialTransactionKey == transactionModel.FinancialTransactionKey && y.UserID == transactionModel.UserId))
                );
        };

        It should_return_an_error_message = () =>
        {
            result.ShouldEqual("An error occured");
        };
    }
}
