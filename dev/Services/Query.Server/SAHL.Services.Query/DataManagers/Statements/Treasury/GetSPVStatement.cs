using SAHL.Core.Data;
using SAHL.Services.Query.Models.Treasury;

namespace SAHL.Services.Query.DataManagers.Statements.Treasury
{
    public class GetSPVStatement : ISqlStatement<SPVDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT
                        SPV.SPVKey AS Id,
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
	
                    FROM [2am].[SPV].[SPV] SPV
                    Inner Join [2AM].dbo.GeneralStatus GS on GS.GeneralStatusKey = spv.GeneralStatusKey
                    Inner Join [2AM].dbo.BankAccount BA on BA.BankAccountKey = spv.BankAccountKey";
        }
    }
}