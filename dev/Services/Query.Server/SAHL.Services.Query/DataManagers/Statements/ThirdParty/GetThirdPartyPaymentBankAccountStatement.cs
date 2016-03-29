using SAHL.Core.Data;
using SAHL.Services.Query.Models.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Query.DataManagers.Statements.ThirdParty
{
    public class GetThirdPartyPaymentBankAccountStatement : ISqlStatement<ThirdPartyPaymentBankAccountDataModel>
    {

        public string GetStatement()
        {
            return @"Select
	                ThirdPartyPaymentBankAccountKey as Id,
	                TPPBA.ThirdPartyKey,
	                TP.Id as ThirdPartyId, 
	                BA.BankAccountKey as BankAccountKey,
	                Coalesce(BA.AccountName, '') as AccountName,
	                Coalesce(BA.ACBBranchCode, '') as BranchCode,
	                Coalesce(B.ACBBranchDescription, '') as BranchName,
	                Coalesce(BA.AccountNumber, '') as AccountNumber,
	                TPPBA.GeneralStatusKey, 
	                Coalesce(BeneficiaryBankCode, '') as BeneficiaryBankCode
                From [2AM].[dbo].[ThirdPartyPaymentBankAccount] TPPBA
                Join [2AM].[dbo].[ThirdParty] TP
	                On TPPBA.ThirdPartyKey = TP.ThirdPartyKey
                Join [2AM].[dbo].BankAccount BA
	                On TPPBA.BankAccountKey = BA.BankAccountKey
                Join [2AM].[dbo].[ACBBranch] B
	                On BA.ACBBranchCode = B.ACBBranchCode";
        }
    }
}
