using System;
using System.Collections.Generic;
using System.Threading;
using SAHL.Core.Exceptions;

namespace SAHL.Core.Logging
{
    public class Logger : ILogger
    {
        public const string AdUser = "ADuser";
        public const string CorrelationId = "CorrelationId";
        public const string Debug = "Debug";
        public const string Error = "Error";
        public const string Exception = "Exception";
        public const string GenericKey = "GenericKey";
        public const string Information = "Info";
        public const string LogLocation = "LogMethodLocation";
        public const string MethodEnter = "MethodEnter";
        public const string MethodException = "MethodException";
        public const string MethodExit = "MethodExit";
        public const string MethodParametersFromAspect = "MethodParametersFromAspect";
        public const string Methodparams = "MethodParameters";
        public const string MethodSuccess = "MethodSuccess";
        public const string PresenterName = "PresenterName";
        public const string StartUp = "Startup";
        public const string ThreadId = "ThreadId";
        public const string Url = "Url";
        public const string ViewName = "ViewName";
        public const string Warning = "Warning";
        public const string X2ActivityName = "ActivityName";
        public const string X2InstanceData = "X2InstanceData";
        public const string X2InstanceId = "InstanceId";
        public const string X2Parameters = "X2Params";
        public const string X2ProcessName = "ProcessName";
        public const string X2WorkflowName = "WorkflowName";
        private static readonly ThreadLocal<IDictionary<string, object>> threadContext = new ThreadLocal<IDictionary<string, object>>(() => new Dictionary<string, object>());

        private readonly ILoggerAppSource applicationSource;
        private readonly IMetricTimerFactory metricsTimerFactory;
        private readonly IRawLogger rawLogger;
        private readonly IRawMetricsLogger rawMetricsLogger;

        public Logger(IRawLogger rawLogger, IRawMetricsLogger rawMetricsLogger, ILoggerAppSource applicationSource, IMetricTimerFactory metricsTimerFactory)
        {
            this.rawLogger = rawLogger;
            this.rawMetricsLogger = rawMetricsLogger;
            this.applicationSource = applicationSource;
            this.metricsTimerFactory = metricsTimerFactory;
        }

        public static IDictionary<string, object> ThreadContext
        {
            get { return threadContext.Value; }
        }

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return ThreadContext;
        }

        public void LogDebug(ILoggerSource loggerSource, string user, string method,
                     string message, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogDebug(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, parameters);
            });
        }

        public void LogError(ILoggerSource loggerSource, string user, string method,
                     string message, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogError(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, parameters);
            });
        }

        public void LogErrorWithException(ILoggerSource loggerSource, string user, string method,
                     string message, System.Exception exception, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, exception, parameters);
            });
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method,
                     string formattedDebugMessage, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedDebug(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedDebugMessage, args);
            });
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method,
                     string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedDebug(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedDebugMessage, parameters, args);
            });
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedError(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, args);
            });
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedError(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, parameters, args);
            });
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, System.Exception exception, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedErrorWithException(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, exception, args);
            });
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, System.Exception exception, IDictionary<string, object> parameters, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedErrorWithException(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, exception, parameters, args);
            });
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedInfo(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, args);
            });
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedInfo(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, parameters, args);
            });
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedWarning(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, args);
            });
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method,
                     string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogFormattedWarning(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     formattedErrorMessage, parameters, args);
            });
        }

        public void LogInfo(ILoggerSource loggerSource, string user, string method,
                     string message, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogInfo(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, parameters);
            });
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogInstantaneousValueMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, value);
                }
            });
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value, IDictionary<string, object> parameters)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogInstantaneousValueMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, value, parameters);
                }
            });
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, startTime, duration);
                }
            });
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration, 
                                IDictionary<string, object> parameters)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, startTime, duration, parameters);
                }
            });
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user,
                    metric, metricsTimerResult.StartTime, metricsTimerResult.Duration);
            });
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric,
                    metricsTimerResult.StartTime, metricsTimerResult.Duration, parameters);
            });
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric,
                    metricsTimerResult.StartTime, metricsTimerResult.Duration);
            });
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric);
            });
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            var result = actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric,
                    metricsTimerResult.StartTime, metricsTimerResult.Duration);
            });
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric);
                }
            });

            return result;
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric,
                    metricsTimerResult.StartTime, metricsTimerResult.Duration, parameters);
            });
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, parameters);
                }
            });
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric, IDictionary<string, object> parameters)
        {
            IMetricTimer metricTimer = null;
            Catcher.Catch.Silently(() =>
            {
                metricTimer = this.metricsTimerFactory.NewTimer();

                if (loggerSource.LogMetrics)
                {
                    metricTimer.Start();
                }
            });
            var result = actionToMetric();
            Catcher.Catch.Silently(() =>
            {
                if (!loggerSource.LogMetrics)
                {
                    return;
                }
                var metricsTimerResult = metricTimer.Stop();
                this.rawMetricsLogger.LogLatencyMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, metricsTimerResult.StartTime, metricsTimerResult.Duration, parameters);
            });
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, parameters);
                }
            });
            return result;
        }

        public void LogOnEnterMethod(ILoggerSource loggerSource, string user, string method,
                     IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogOnEnterMethod(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     parameters);
            });
        }

        public void LogOnExitMethod(ILoggerSource loggerSource, string user, string method,
                     IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogOnExitMethod(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     parameters);
            });
        }

        public void LogOnMethodException(ILoggerSource loggerSource, string user, string method,
                     System.Exception exception, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogOnMethodException(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     exception, parameters);
            });
        }

        public void LogOnMethodSuccess(ILoggerSource loggerSource, string user, string method,
                     IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogOnMethodSuccess(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     parameters);
            });
        }
        public void LogStartup(ILoggerSource loggerSource, string user, string method,
                     string message, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogStartup(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, parameters);
            });
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric);
                }
            });
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric, IDictionary<string, object> parameters)
        {
            Catcher.Catch.Silently(() =>
            {
                if (loggerSource.LogMetrics)
                {
                    this.rawMetricsLogger.LogThroughputMetric(loggerSource.Name, this.applicationSource.ApplicationName, user, metric, parameters);
                }
            });
        }

        public void LogWarning(ILoggerSource loggerSource, string user, string method,
                     string message, IDictionary<string, object> parameters = null)
        {
            Catcher.Catch.Silently(() =>
            {
                this.rawLogger.LogWarning(loggerSource.LogLevel, loggerSource.Name, this.applicationSource.ApplicationName, user, method,
                     message, parameters);
            });
        }
        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            foreach (var kvp in threadContext)
            {
                if (!ThreadContext.ContainsKey(kvp.Key))
                {
                    ThreadContext.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    ThreadContext[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}