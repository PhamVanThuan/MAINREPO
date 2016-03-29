using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Origination
{
    public interface IStageAndActivityMessages : IX2WorkflowService
    {
        void ReturnToProcessorFromCredit(IDomainMessageCollection messages, Int64 instanceID, out int workflowHistoryID);

        void ReturnToProcessor(IDomainMessageCollection messages, Int64 instanceID, out int workflowHistoryID);

        void DeclinedByCredit(IDomainMessageCollection messages, Int64 instanceID, string callingMap, out int workflowHistoryID);

        void IsMotivate(IDomainMessageCollection messages, Int64 instanceID, out int workflowHistoryID);
    }
}