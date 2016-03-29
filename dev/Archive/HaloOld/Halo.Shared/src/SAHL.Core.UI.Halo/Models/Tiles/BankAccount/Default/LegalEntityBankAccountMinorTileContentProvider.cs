using System;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.BankAccount.Default
{
    public class LegalEntityBankAccountMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityBankAccountMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return String.Format(@"select ba.AccountName, ba.AccountNumber, bk.ACBBankDescription as Bank, ba.ACBBranchCode as BranchCode, at.ACBTypeDescription as AccountType
                                    from [2AM].[dbo].[LegalEntity] le
                                    join (select leba.LegalEntityKey, max(ba.BankAccountKey) as BankAccountKey
                                              from [2AM].[dbo].[LegalEntityBankAccount] leba
                                              join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                                    and ba.ACBTypeNumber in (0, 1, 2)
                                              where leba.GeneralStatusKey = 1
                                              group by leba.LegalEntityKey) leba on leba.LegalEntityKey = le.LegalEntityKey
                                    join [2AM].[dbo].[BankAccount] ba on ba.BankAccountKey = leba.BankAccountKey
                                    join [2AM].[dbo].[ACBBranch] br on br.ACBBranchCode = ba.ACBBranchCode
                                    join [2AM].[dbo].[ACBBank] bk on bk.ACBBankCode = br.ACBBankCode
                                    join [2AM].[dbo].[ACBType] at on at.ACBTypeNumber = ba.ACBTypeNumber
                                    where le.LegalEntityKey = {0}", businessKey.Key);
        }
    }
}