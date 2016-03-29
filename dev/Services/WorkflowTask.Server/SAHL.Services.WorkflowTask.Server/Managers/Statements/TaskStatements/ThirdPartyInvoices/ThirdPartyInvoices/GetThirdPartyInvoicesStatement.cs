using SAHL.Core.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.ThirdPartyInvoices.ThirdPartyInvoices
{
    internal class GetThirdPartyInvoicesStatement : TaskBaseStatement
    {
        public GetThirdPartyInvoicesStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"SELECT I.ID AS InstanceId
                        ,TagIds = (" + tagStatementSubQuery + @")
			            ,TP.InvoiceNumber as 'invoice number'
			            ,TP.AccountKey as account
                        ,[Subject]
			            ,spv_parent.Description As 'spv'
			            ,CONCAT('R ', CAST(TP.TotalAmountIncludingVAT AS MONEY)) as 'amount'
                        ,WL.ADUserName as 'Assigned To'
						,FORMAT(TP.ReceivedDate,'yyyy/MM/dd h:mm:ss tt') as 'received date'                        
                        ,TP.SahlReference as Reference
                        ,ST.GenericKey
                        ,ProcessName = PRO.Name
                        ,W.GenericKeyTypeKey
                        ,WorkflowName = W.Name
                    FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
                    INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) 
                        ON WL.InstanceID = I.ID
                    INNER JOIN [X2].[X2DATA].[Third_Party_Invoices] X2D WITH (NOLOCK) 
                        ON X2D.InstanceID = I.ID
                    INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) 
                        ON I.StateID = S.ID
                    INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) 
                        ON S.WorkflowID = W.ID
                    INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) 
                        ON w.ProcessID = PRO.ID
                    INNER JOIN [X2].[X2DATA].Third_Party_Invoices ST 
                        ON ST.InstanceID = I.ID
		            INNER JOIN [2AM].[dbo].[ThirdPartyInvoice] TP  
                        ON TP.[ThirdPartyInvoiceKey] = X2D.[ThirdPartyInvoiceKey]
		            INNER JOIN [2AM].[dbo].Account acc 
                        ON acc.AccountKey = TP.AccountKey
		            INNER JOIN [2AM].spv.SPV spv  
                        ON spv.SPVKey = acc.SPVKey
                    INNER JOIN [2AM].spv.SPV spv_parent
                        ON spv.ParentSPVKey = spv_parent.spvKey
                    Where PRO.ViewableOnUserInterfaceVersion LIKE '3%'
								AND S.Name IN @WorkFlowStateNames
								AND W.Name = @WorkFlowName
								AND PRO.Name = @BusinessProcessName
								AND WL.ADUserName IN @UsernameAndRoles
					ORDER by TP.ReceivedDate Desc
                    ";
        }
    }
}
