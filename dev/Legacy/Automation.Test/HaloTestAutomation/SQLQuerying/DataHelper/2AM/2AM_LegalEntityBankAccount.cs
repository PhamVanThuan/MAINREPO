using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public IEnumerable<Automation.DataModels.LegalEntityBankAccount> GetLegalEntityBankAccount(int legalEntityKey)
        {
            return dataContext.Query<Automation.DataModels.LegalEntityBankAccount>(string.Format(@"select * from [2am].dbo.legalentitybankaccount where legalentitykey = {0}", legalEntityKey));
        }

        public IEnumerable<Automation.DataModels.LegalEntityBankAccount> GetLegalEntityBankAccount()
        {
            return dataContext.Query<Automation.DataModels.LegalEntityBankAccount>(string.Format(@"select * from [2am].dbo.legalentitybankaccount"));
        }

        public IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntitiesLinkedToABankAccount(int bankAccountKey)
        {
            return dataContext.Query<Automation.DataModels.LegalEntity>(string.Format(@"select le.* from [2am].dbo.LegalEntityBankAccount leba join
                    [2am].dbo.Legalentity le on leba.LegalEntityKey = le.LegalEntityKey
                    where leba.BankAccountKey = {0}", bankAccountKey));
        }

        public IEnumerable<Automation.DataModels.BankAccount> GetBankAccountsLinkedToALegalEntities(int legalEntityKey)
        {
            return dataContext.Query<Automation.DataModels.BankAccount>(string.Format(@"select ba.*, br.*, b.* from [2am].dbo.LegalEntityBankAccount leba join
                    [2am].dbo.BankAccount ba on leba.BankAccountKey = ba.BankAccountKey join
                    [2am].dbo.ACBBranch br on ba.ACBBranchCode = br.ACBBranchCode join
                    [2am].dbo.ACBBank b on br.ACBBankCode = b.ACBBankCode
                    where leba.LegalEntityKey = {0}", legalEntityKey));
        }
    }
}