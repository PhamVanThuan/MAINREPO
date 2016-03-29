using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.BankAccountDomain.Models;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class GetBankAccountStatement : ISqlStatement<BankAccountDataModel>
    {
        public int BankAccountType { get; protected set; }
        public string BranchCode { get; protected set; }
        public string AccountNumber { get; protected set; }

        public GetBankAccountStatement(BankAccountModel bankAccount)
        {
            this.BankAccountType = (int)bankAccount.AccountType;
            this.BranchCode = bankAccount.BranchCode;
            this.AccountNumber = bankAccount.AccountNumber;
        }

        public string GetStatement()
        {
            var query = @"SELECT [BankAccountKey]
                          ,[ACBBranchCode]
                          ,[AccountNumber]
                          ,[ACBTypeNumber]
                          ,[AccountName]
                          ,[UserID]
                          ,[ChangeDate]
                      FROM [BankAccount]
                      WHERE
                       [ACBBranchCode] = @BranchCode
                      AND
                       [AccountNumber] = @AccountNumber
                      AND
                       [ACBTypeNumber] = @BankAccountType";

            return query;
        }
    }
}
