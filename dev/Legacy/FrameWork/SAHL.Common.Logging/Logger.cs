using System.Collections.Generic;
using System.Threading;

namespace SAHL.Common.Logging
{
    public static class Logger
    {
        public const string METHODPARAMS = "MethodParameters";
        public const string METHODPARAMSFROMASPECT = "MethodParametersFromAspect";
        public const string ADUSERNAME = "ADUserName";
        public const string LOGLOCATION = "LogMethodLocation";
        public const string EXCEPTION = "Exception";
        public const string CORRELATIONID = "CorrelationId";
        public const string GENERICKEY = "GenericKey";
        public const string VIEWNAME = "ViewName";
        public const string PRESENTERNAME = "PresenterName";
        public const string URL = "Url";

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