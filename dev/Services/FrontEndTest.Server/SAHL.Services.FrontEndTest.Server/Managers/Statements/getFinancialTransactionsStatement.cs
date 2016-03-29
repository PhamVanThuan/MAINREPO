using SAHL.Core.Data;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class GetFinancialTransactionsStatement : ISqlStatement<GetFinancialTransactionsQueryResult>
    {
        public int FinancialServiceKey { get; protected set; }

        public GetFinancialTransactionsStatement(int financialServiceKey)
        {
            this.FinancialServiceKey = FinancialServiceKey;
        }

        public string GetStatement()
        {
            return @"Select * from [2am].fin.FinancialTransaction Where FinancialServiceKey = @FinancialServiceKey";
        }
    }
}