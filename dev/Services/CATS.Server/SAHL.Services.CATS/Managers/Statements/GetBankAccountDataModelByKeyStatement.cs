using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.CATS.Managers.Statements
{
    public class GetBankAccountDataModelByKeyStatement : ISqlStatement<BankAccountDataModel>
    {
        public int BankAccountKey { get; protected set; }
        public GetBankAccountDataModelByKeyStatement(int bankAccountKey)
        {
            this.BankAccountKey = bankAccountKey;
        }
        public string GetStatement()
        {
            return @"SELECT [BankAccountKey]
                      ,[ACBBranchCode]
                      ,[AccountNumber]
                      ,[ACBTypeNumber]
                      ,[AccountName]
                      ,[UserID]
                      ,[ChangeDate]
                  FROM [2AM].[dbo].[BankAccount] 
                  where BankAccountKey = @BankAccountKey";
        }
    }
}
