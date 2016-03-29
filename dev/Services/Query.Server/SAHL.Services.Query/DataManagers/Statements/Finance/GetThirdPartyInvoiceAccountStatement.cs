
using SAHL.Core.Data;
using SAHL.Services.Query.Models.Finance;

namespace SAHL.Services.Query.DataManagers.Statements.Finance
{
    public class GetThirdPartyInvoiceAccountStatement : ISqlStatement<ThirdPartyInvoiceAccountDataModel>
    {
        public string GetStatement()
        {
            return @"Select Top 1 TPI.ThirdPartyInvoiceKey as Id,
		A.AccountKey AS AccountKey,
	    AccountStatusKey,
	    ChangeDate,
	    CloseDate,
	    FixedPayment,
	    OpenDate,
        [spv].SPVKey as SPVKey,
		SPV.Description as [SpvDescription],
		coalesce(Acc.eMapName, 'Not Assigned') as WorkflowProcess, 
	    coalesce(Acc.eStageName, 'Not Assigned') as WorkflowStage,
		coalesce(LA.UserName, 'Not Assigned') as AssignedTo
    From [2AM].[dbo].[ThirdPartyInvoice] TPI
	Inner Join [2AM].[dbo].[Account] A
		On TPI.AccountKey = A.AccountKey
	Inner Join [2am].[SPV].[SPV] SPV
		On A.SPVKey = SPV.SPVKey
	Left Join [EventProjection].[projection].[CurrentlyAssignedUserForInstance] LA
		On LA.GenericKey = TPI.ThirdPartyInvoiceKey
		And LA.GenericKeyTypeKey = 54
    left join (
	        Select 
		        eFolderId,
		        coalesce(f.eFolderName, '''') as AccountKey,
		        ROW_NUMBER() OVER(PARTITION BY coalesce(f.eFolderName, '''') ORDER BY f.ecreationtime DESC) AS Row,
				f.eMapName,
				f.eStageName
	        From [e-work].dbo.eFolder f 
	        Where f.eMapName = 'LossControl' and ISNUMERIC(coalesce(f.eFolderName, ''''))=1 			
	        ) as Acc on a.AccountKey=Acc.AccountKey and Acc.Row = 1";
        }
    }
}
