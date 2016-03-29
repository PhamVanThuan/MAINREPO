using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging
{
    public interface IRawLogger : IParticipatesInThreadLocalStorage
    {
        void LogDebug(LogLevel logLevel, string source, string application, string user, string method,
                     string message, IDictionary<string, object> parameters = null);

        void LogError(LogLevel logLevel, string source, string application, string user, string method,
                     string message, IDictionary<string, object> parameters = null);

        void LogErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
                     string message, Exception exception, IDictionary<string, object> parameters = null);

        void LogFormattedDebug(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedDebugMessage, params object[] args);

        void LogFormattedDebug(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args);

        void LogFormattedError(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, params object[] args);

        void LogFormattedError(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogFormattedErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, Exception exception, params object[] args);

        void LogFormattedErrorWithException(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, Exception exception, IDictionary<string, object> parameters, params object[] args);

        void LogFormattedInfo(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, params object[] args);

        void LogFormattedInfo(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogFormattedWarning(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, params object[] args);

        void LogFormattedWarning(LogLevel logLevel, string source, string application, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogInfo(LogLevel logLevel, string source, string application, string user, string method,
                     string message, IDictionary<string, object> parameters = null);

        void LogOnEnterMethod(LogLevel logLevel, string source, string application, string user, string method,
                     IDictionary<string, object> parameters = null);

        void LogOnExitMethod(LogLevel logLevel, string source, string application, string user, string method,
                     IDictionary<string, object> parameters = null);

        void LogOnMethodException(LogLevel logLevel, string source, string application, string user, string method,
                     Exception exception, IDictionary<string, object> parameters = null);

        void LogOnMethodSuccess(LogLevel logLevel, string source, string application, string user, string method,
                     IDictionary<string, object> parameters = null);

        void LogStartup(LogLevel logLevel, string source, string application, string user, string method,
                     string message, IDictionary<string, object> parameters = null);

        void LogWarning(LogLevel logLevel, string source, string application, string method,
                     string message, string user, IDictionary<string, object> parameters = null);
    }
}