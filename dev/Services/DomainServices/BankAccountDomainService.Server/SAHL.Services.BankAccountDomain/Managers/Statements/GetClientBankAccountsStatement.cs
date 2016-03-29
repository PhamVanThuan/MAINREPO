using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class GetClientBankAccountsStatement : ISqlStatement<LegalEntityBankAccountDataModel>
    {
        public int ClientKey { get; protected set; }

        public int BankAccountKey { get; protected set; }

        public GetClientBankAccountsStatement(int clientKey, int bankAccountKey)
        {
            this.BankAccountKey = bankAccountKey;
            this.ClientKey = clientKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT [LegalEntityBankAccountKey], [LegalEntityKey], [BankAccountKey], [GeneralStatusKey], [UserID], [ChangeDate] 
                            FROM [LegalEntityBankAccount] leba WHERE LegalEntityKey = @ClientKey AND BankAccountKey = @BankAccountKey";
            return query;
        }
    }
}