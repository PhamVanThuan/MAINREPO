using System.Collections.Generic;
using System.Threading;

namespace SAHL.Common.Logging
{
    public static class Metrics
    {
        public const string VIEWNAME = "ViewName";
        public const string PRESENTERNAME = "Presenter";
        public const string RESPONSECORRELATIONID = "ResponseCorrelationId";
        public const string CORRELATIONID = "CorrelationId";
        public const string UISTATEMENTNAME = "UIStatementName";
        public const string X2INSTANCEDATA = "X2InstanceData";
        public const string X2PARAMS = "X2Params";
        public const string GENERICKEY = "GenericKey";
        public const string X2INSTANCEID = "InstanceId";
        public const string X2ACTIVITYNAME = "ActivityName";
        public const string X2WORKFLOWNAME = "WorkflowName";
        public const string X2PROCESSNAME = "ProcessName"; 
        public const string THREADID = "ThreadId";
        public const string SessionID = "SessionId";

        private static ThreadLocal<Dictionary<string, object>> threadContext = new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());

        public static Dictionary<string, object> ThreadContext
        {
            get
            {
                return threadContext.Value;
            }
        }
    }
}