using SAHL.Core.Logging;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Web.Specs
{
    public class FakeLogger : ILogger
    {
        public void LogOnEnterMethod(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogOnMethodSuccess(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogOnExitMethod(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogOnMethodException(ILoggerSource loggerSource, string user, string method, Exception exception, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogInfo(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogWarning(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogDebug(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method, string formattedDebugMessage, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method, string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogError(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogErrorWithException(ILoggerSource loggerSource, string user, string method, string message, Exception exception, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, Exception exception, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, Exception exception, 
                                                    IDictionary<string, object> parameters, params object[] args)
        {
            //Fake logger, empty by design
        }

        public void LogStartup(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            //Fake logger, empty by design
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration)
        {
            //Fake logger, empty by design
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters)
        {
            //Fake logger, empty by design
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            actionToMetric();
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            actionToMetric();
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value)
        {
            //Fake logger, empty by design
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value, IDictionary<string, object> parameters)
        {
            //Fake logger, empty by design
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric)
        {
            //Fake logger, empty by design
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric, IDictionary<string, object> parameters)
        {
            //Fake logger, empty by design
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            actionToMetric();
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            actionToMetric();
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric)
        {
            return default(T);
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric, IDictionary<string, object> parameters)
        {
            return default(T);
        }

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return null;
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            //Fake logger, empty by design
        }
    }
}