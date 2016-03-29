using SAHL.Core.Data;

namespace SAHL.Shared.BusinessModel.Managers.Statements
{
    public class DoesTransactionTypeExistStatement : ISqlStatement<int>
    {
        public DoesTransactionTypeExistStatement(int transactionTypeKey)
        {
            this.TransactionTypeKey = transactionTypeKey;
        }

        public int TransactionTypeKey { get; protected set; }

        public string GetStatement()
        {
            var sql = @"select
	                        count(*)
                        from
	                        [2am].fin.TransactionType
                        where
	                        TransactionTypeKey = @TransactionTypeKey";

            return sql;
        }
    }
}