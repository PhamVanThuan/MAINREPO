using System;
using System.Collections.Generic;
using System.Threading;

namespace SAHL.Core.Logging
{
    public class NullRawLogger : IRawLogger
    {
        private static ThreadLocal<IDictionary<string, object>> threadContext = new ThreadLocal<IDictionary<string, object>>(() => new Dictionary<string, object>());

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return NullRawLogger.threadContext.Value;
        }

        public void LogDebug(LogLevel logLevel, string source, string application, string user, string method,
            string message, IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogError(LogLevel logLevel, string source, string application, string user, string method,
            string message, IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
            string message, Exception exception, IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedDebug(LogLevel logLevel, string source, string application, string user, string method,
            string formattedDebugMessage, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedDebug(LogLevel logLevel, string source, string application, string user, string method,
            string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedError(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedError(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, Exception exception, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, Exception exception, IDictionary<string, object> parameters, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedInfo(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedInfo(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedWarning(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogFormattedWarning(LogLevel logLevel, string source, string application, string user, string method,
            string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogInfo(LogLevel logLevel, string source, string application, string user, string method, string message,
            IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogOnEnterMethod(LogLevel logLevel, string source, string application, string user, string method,
            IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogOnExitMethod(LogLevel logLevel, string source, string application, string user, string method,
            IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogOnMethodException(LogLevel logLevel, string source, string application, string user, string method,
            Exception exception, IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogOnMethodSuccess(LogLevel logLevel, string source, string application, string user, string method,
            IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogStartup(LogLevel logLevel, string source, string application, string user, string method, string message,
            IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void LogWarning(LogLevel logLevel, string source, string application, string method, string message,
            string user, IDictionary<string, object> parameters = null)
        {
            //intentionally left empty, so that this does nothing.
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            NullRawLogger.threadContext.Value = threadContext;
        }
    }
}