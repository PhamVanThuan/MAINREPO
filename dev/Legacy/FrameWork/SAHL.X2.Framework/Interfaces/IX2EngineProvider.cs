using System;
using System.Collections.Generic;

namespace SAHL.X2.Framework.Interfaces
{
    public interface IX2Provider
    {
        X2ResponseBase LogIn();

        X2ResponseBase LogOut(string SessionID);

        X2ResponseBase CreateWorkFlowInstance(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data);

        X2ResponseBase CreateWorkFlowInstanceWithComplete(string SessionID, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data);

        X2ResponseBase CreateCompleteActivity(string SessionID, long InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data = null);

        X2ResponseBase StartActivity(string SessionID, Int64 InstanceID, string ActivityName, bool IgnoreWarnings, object Data);

        X2ResponseBase CompleteActivity(string SessionID, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);

        X2ResponseBase CancelActivity(string SessionID, Int64 InstanceID, string ActivityName);

        X2ResponseBase CommandActivity(string Command, string CommandArgs, string SessionID);

        X2ResponseBase ProcessListActivity(List<ListRequestItem> ItemList);

        X2ResponseBase AquirePublisherMode(string ADUser, string ProcessName);

        X2ResponseBase ReleasePublisherMode(string ADUser);

        X2ResponseBase ClearConnectionPool();

        X2ResponseBase RefreshCacheInX2NodeProcess(Object data);
    }
}