using SAHL.Core.Data;
using SAHL.Services.Query.Models.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Query.DataManagers.Statements.ThirdParty
{
    public class GetThirdPartyBankAccountStatement : ISqlStatement<ThirdPartyBankAccountDataModel>
    {
        public string GetStatement()
        {
            return @"Select
	                LEBA.BankAccountKey as Id,
                    TP.Id As ThirdPartyId,
	                TP.ThirdPartyKey As ThirdPartyKey,
	                LE.LegalEntityKey As LegalEntityKey,
	                BA.BankAccountKey As BankAccountKey,
	                Coalesce (BA.AccountNumber,'')  As AccountNumber,
	                Coalesce (BA.ACBBranchCode,'') As BranchCode,
	                Coalesce (BA.AccountName,'') As AccountName,
	                Coalesce (BK.ACBBankDescription ,'')As BankingInstitute
                From [2AM].[dbo].[ThirdParty] TP
                Join [2AM].[dbo].[LegalEntity] LE
                On TP.LegalEntityKey = LE.LegalEntityKey

                Left Join (Select leba.LegalEntityKey, Max(BA.BankAccountKey) As BankAccountKey
                                From [2AM].[dbo].[LegalEntityBankAccount] LEBA
                                Join [2AM].[dbo].[BankAccount] BA
				                On ba.BankAccountKey = LEBA.BankAccountKey
                                And BA.ACBTypeNumber in (0, 1, 2)
                                Where LEBA.GeneralStatusKey = 1
                                Group By LEBA.LegalEntityKey) LEBA On LEBA.LegalEntityKey = LE.LegalEntityKey
                Left Join [2AM].[dbo].[BankAccount] BA 
	                On BA.BankAccountKey = LEBA.BankAccountKey
                Left Join [2AM].[dbo].[ACBBranch] BR
                   On BR.ACBBranchCode = BA.ACBBranchCode
                Left Join [2AM].[dbo].[ACBBank] BK
                  On BK.ACBBankCode = BR.ACBBankCode";
        }
    }
}
