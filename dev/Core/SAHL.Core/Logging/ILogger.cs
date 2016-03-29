using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging
{
    public interface ILogger : IParticipatesInThreadLocalStorage
    {
        void LogOnEnterMethod(ILoggerSource loggerSource, string user,
                     string method, IDictionary<string, object> parameters = null);

        void LogOnMethodSuccess(ILoggerSource loggerSource, string user,
                     string method, IDictionary<string, object> parameters = null);

        void LogOnExitMethod(ILoggerSource loggerSource, string user,
                     string method, IDictionary<string, object> parameters = null);

        void LogOnMethodException(ILoggerSource loggerSource, string user,
                     string method, Exception exception, IDictionary<string, object> parameters = null);

        void LogStartup(ILoggerSource loggerSource, string user,
                     string method, string message, IDictionary<string, object> parameters = null);

        void LogInfo(ILoggerSource loggerSource, string user,
                     string method, string message, IDictionary<string, object> parameters = null);

        void LogFormattedInfo(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, params object[] args);

        void LogFormattedInfo(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogWarning(ILoggerSource loggerSource, string user,
                     string method, string message, IDictionary<string, object> parameters = null);

        void LogFormattedWarning(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, params object[] args);

        void LogFormattedWarning(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogDebug(ILoggerSource loggerSource, string user,
                     string method, string message, IDictionary<string, object> parameters = null);

        void LogFormattedDebug(ILoggerSource loggerSource, string user,
                     string method, string formattedDebugMessage, params object[] args);

        void LogFormattedDebug(ILoggerSource loggerSource, string user,
                     string method, string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args);

        void LogError(ILoggerSource loggerSource, string user,
                     string method, string message, IDictionary<string, object> parameters = null);

        void LogFormattedError(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, params object[] args);

        void LogFormattedError(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args);

        void LogErrorWithException(ILoggerSource loggerSource, string user,
                     string method, string message, Exception exception, IDictionary<string, object> parameters = null);

        void LogFormattedErrorWithException(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, Exception exception, params object[] args);

        void LogFormattedErrorWithException(ILoggerSource loggerSource, string user,
                     string method, string formattedErrorMessage, Exception exception, IDictionary<string, object> parameters, params object[] args);

        void LogLatencyMetric(ILoggerSource loggerSource, string user,
                     string metric, DateTime startTime, TimeSpan duration);

        void LogLatencyMetric(ILoggerSource loggerSource, string user,
                     string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters);

        void LogLatencyMetric(ILoggerSource loggerSource, string user,
                     string metric, Action actionToMetric);

        void LogLatencyMetric(ILoggerSource loggerSource, string user,
                     string metric, Action actionToMetric, IDictionary<string, object> parameters);

        void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user,
                     string metric, int value);

        void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user,
                     string metric, int value, IDictionary<string, object> parameters);

        void LogThroughputMetric(ILoggerSource loggerSource, string user,
                     string metric);

        void LogThroughputMetric(ILoggerSource loggerSource, string user,
                     string metric, IDictionary<string, object> parameters);

        void LogMethodMetric(ILoggerSource loggerSource, string user,
                     string metric, Action actionToMetric);

        T LogMethodMetric<T>(ILoggerSource loggerSource, string user,
                     string metric, Func<T> actionToMetric);

        void LogMethodMetric(ILoggerSource loggerSource, string user,
                     string metric, Action actionToMetric, IDictionary<string, object> parameters);

        T LogMethodMetric<T>(ILoggerSource loggerSource, string user,
                     string metric, Func<T> actionToMetric, IDictionary<string, object> parameters);
    }
}