using SAHL.Core.Data;
using SAHL.Services.Query.Models.Account;

namespace SAHL.Services.Query.DataManagers.Statements.Account
{
    public class GetAccountSpvStatement : ISqlStatement<AccountSpvDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT Top 1
                        SPV.SPVKey AS Id,
	                    A.AccountKey As AccountKey,
                        Coalesce (SPV.Description,'')  As SPVDescription,
                        Coalesce (SPV.ReportDescription, '') As ReportDescription,	      
                        Coalesce (SPV.CreditProviderNumber, '') As CreditProviderNumber,	              
                        Coalesce (SPV.RegistrationNumber, '') As RegistrationNumber,	                 
                        SPV.BankAccountKey,
                        SPV.GeneralStatusKey,
                        SPV.ParentSPVKey AS ParentSPVKey,
                        GS.Description AS GeneralStatus,                        
                        Coalesce(BA.AccountName, '') as AccountName,
                        Coalesce(BA.ACBBranchCode, '') as BranchCode,
                        Coalesce(BA.AccountNumber, '') as AccountNumber
                    FROM [2AM].[dbo].[Account] A
                    Inner Join [2am].[SPV].[SPV] SPV
	                    On A.SPVKey = SPV.SPVKey
                    Inner Join [2AM].dbo.GeneralStatus GS 
	                    On GS.GeneralStatusKey = spv.GeneralStatusKey
                    Inner Join [2AM].dbo.BankAccount BA 
	                    On BA.BankAccountKey = spv.BankAccountKey";
        }
    }
}
