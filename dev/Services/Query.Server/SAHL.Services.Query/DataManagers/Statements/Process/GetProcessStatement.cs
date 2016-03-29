using SAHL.Core.Data;
using SAHL.Services.Query.Models.Process;

namespace SAHL.Services.Query.DataManagers.Statements.Process
{
    public class GetProcessStatement : ISqlStatement<ProcessDataModel>
    {
        public string GetStatement()
        {
            return @"Select 
	                    f.eFolderId as Id,
	                    f.eMapName as Process, 
	                    f.eStageName as Stage, 
	                    coalesce(f.eFolderName, '') as AccountKey
                    From [e-work].dbo.eFolder F
                    Inner Join (
	                    Select 
		                    eFolderId,
		                    coalesce(f.eFolderName, '') AccountKey,
		                    ROW_NUMBER() OVER(PARTITION BY coalesce(f.eFolderName, '') ORDER BY f.ecreationtime DESC) AS Row
	                    From [e-work].dbo.eFolder f 
	                    Where f.eMapName = 'LossControl'	
	                    ) as Acc
                    On Acc.eFolderID = F.eFolderID
                    Where Acc.Row = 1";
        }
    }
}