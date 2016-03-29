using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetThirdPartyInvoicesInstanceDetailsQueryStatement : IServiceQuerySqlStatement<GetThirdPartyInvoicesInstanceDetailsQuery, GetThirdPartyInvoicesInstanceDetailsQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT i.ID as InstanceID, i.WorkFlowID, i.ParentInstanceID, i.Name, i.Subject, i.WorkFlowProvider,
								i.StateID, i.CreatorADUserName, i.CreationDate, i.StateChangeDate, i.DeadlineDate, i.ActivityDate,
								i.ActivityADUserName, i.ActivityID, i.Priority, i.SourceInstanceID, i.ReturnActivityID, w.ProcessID,
								w.WorkFlowAncestorID, w.Name AS WorkFlowName, w.CreateDate, w.StorageTable, w.StorageKey, w.IconID, w.DefaultSubject,
								w.GenericKeyTypeKey, s.WorkFlowID AS StateWorkFlowID, s.Name AS StateName, s.Type, s.ForwardState, s.Sequence,
								s.ReturnWorkflowID, s.ReturnActivityID AS StateReturnActivityID, t.ThirdPartyInvoiceKey, t.AccountKey, t.ThirdPartyTypeKey, t.GenericKey
								FROM
								[X2].[X2DATA].[Third_Party_Invoices] t
								join x2.x2.instance i on t.instanceid=i.id
								join x2.x2.state s on i.stateid=s.id
								join x2.x2.workflow w on s.workflowid=w.id
								where t.ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey and i.ParentInstanceid is null
								order by 1 desc";
        }
    }
}
