using SAHL.Core.Data;
using SAHL.Shared.BusinessModel.Managers.Statements;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Managers
{
    public class TransactionDataManager : ITransactionDataManager
    {
        private IDbFactory dbFactory;

        public TransactionDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesFinancialTransactionKeyExist(int financialTransactionKey)
        {
            var query = new DoesFinancialTransactionKeyExistStatement(financialTransactionKey);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return (db.SelectOne<int>(query) > 0);
            }
        }

        public bool DoesTransactionTypeExist(int transactionTypeKey)
        {
            var doesTransactionTypeExistStatement = new DoesTransactionTypeExistStatement(transactionTypeKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<int>(doesTransactionTypeExistStatement);

                return result > 0;
            }
        }

        public string PostTransaction(PostTransactionModel transactionModel)
        {
            var postTransactionStatement = new PostTransactionStatement(
                                                transactionModel.FinancialServiceKey, transactionModel.TransactionTypeKey,
                                                transactionModel.EffectiveDate, transactionModel.Amount,
                                                transactionModel.Reference, transactionModel.UserId);

            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var result = db.SelectOne<string>(postTransactionStatement);

                return result;
            }
        }

        public string PostReversalTransaction(ReversalTransactionModel transactionModel)
        {
            var query = new PostReversalTransactionStatement(transactionModel.FinancialTransactionKey, transactionModel.UserId);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.SelectOne<string>(query);
            }
        }
    }
}