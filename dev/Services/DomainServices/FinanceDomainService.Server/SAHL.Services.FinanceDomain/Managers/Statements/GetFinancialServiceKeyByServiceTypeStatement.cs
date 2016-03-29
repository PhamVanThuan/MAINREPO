using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinanceDomain.Enum;

namespace SAHL.Services.FinanceDomain.Managers.Statements
{
    public class GetFinancialServiceKeyByServiceTypeStatement : ISqlStatement<int?>
    {
        public int AccountNumber { get; protected set; }
        public int FinancialServiceTypeKey { get; protected set; }

        public GetFinancialServiceKeyByServiceTypeStatement(int accountNumber, FinancialServiceTypeEnum financialServiceTypeKey)
        {
            this.AccountNumber = accountNumber;
            this.FinancialServiceTypeKey = (int)financialServiceTypeKey;
        }

        public string GetStatement()
        {
            return @"SELECT FinancialServiceKey FROM [2AM].[dbo].[FinancialService]
                    WHERE AccountKey = @AccountNumber AND FinancialServiceTypeKey = @FinancialServiceTypeKey";
        }
    }
}
