using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.Origination
{
    public interface IStageAndActivityMessages
    {
        IX2ReturnData ReturnToProcessor(Int64 InstanceID, out int WorkflowHistoryID);
        IX2ReturnData DeclinedByCredit(Int64 InstanceID, string CallingMap, out int WorkflowHistoryID);
        IX2ReturnData IsMotivate(Int64 InstanceID, out int WorkflowHistoryID);
    }
}
