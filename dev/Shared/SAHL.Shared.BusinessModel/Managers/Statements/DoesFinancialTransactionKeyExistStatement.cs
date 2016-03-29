using SAHL.Core.Data;

namespace SAHL.Shared.BusinessModel.Managers.Statements
{
    public class DoesFinancialTransactionKeyExistStatement : ISqlStatement<int>
    {
        public int FinancialTransactionKey { get; protected set; }

        public DoesFinancialTransactionKeyExistStatement(int financialTransactionKey)
        {
            FinancialTransactionKey = financialTransactionKey;
        }

        public string GetStatement()
        {
            return @"SELECT COUNT(*) FROM [2AM].fin.FinancialTransaction WHERE FinancialTransactionKey = @FinancialTransactionKey";
        }
    }
}
