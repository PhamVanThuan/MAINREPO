using System;
using System.Collections.Generic;

namespace DomainService2.SharedServices
{
    public interface IX2WorkflowService
    {
        void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data);

        IDictionary<string, object> GetX2DataRow(long instanceID);

        void ArchiveValuationsFromSourceInstanceID(Int64 instanceID, string adUser, int applicationKey);

        void ArchiveQuickCashFromSourceInstanceID(Int64 instanceID, string adUser, int applicationKey);

        bool HasInstancePerformedActivity(Int64 instanceID, string activityName);
    }
}