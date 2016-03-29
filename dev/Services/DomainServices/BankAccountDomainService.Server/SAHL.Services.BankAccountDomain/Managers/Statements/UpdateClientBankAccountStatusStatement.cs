using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.BankAccountDomain.Managers.Statements
{
    public class UpdateClientBankAccountStatusStatement : ISqlStatement<LegalEntityBankAccountDataModel>
    {
        public int ClientBankAccountKey { get; protected set; }
        public int StatusKey { get; protected set; }

        public UpdateClientBankAccountStatusStatement(int clientBankAccountKey, GeneralStatus statusKey)
        {
            this.ClientBankAccountKey = clientBankAccountKey;
            this.StatusKey = (int)statusKey;
        }

        public string GetStatement()
        {
            var query = @"UPDATE 
                            [LegalEntityBankAccount] 
                        SET 
                            [GeneralStatusKey] = @StatusKey
                        WHERE [LegalEntityBankAccountKey] = @ClientBankAccountKey";

            return query;
        }
    }
}
