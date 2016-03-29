using SAHL.Communication;
using SAHL.Shared.Messages;
using SAHL.Shared.Messages.Metrics;
using System;
using System.Collections.Generic;

namespace SAHL.Common.Logging
{
    public class MessageBusLogger : ILogger
    {
        const string ERROR = "LogError";
        const string WARNING = "LogWarning";
        const string INFORMATION = "LogInformation";
        const string DEBUG = "LogDebug";

        private IMessageBus messageBus = null;
        private string applicationName = String.Empty;
        private int logLevel;

        public MessageBusLogger(IMessageBus messageBus, MessageBusLoggerConfiguration messageBusLoggerConfiguration)
        {
            this.messageBus = messageBus;
            this.applicationName = messageBusLoggerConfiguration.ApplicationName;
            this.logLevel = messageBusLoggerConfiguration.LogLevel;
        }

        public void LogOnEnterMethod(string methodName, Dictionary<string, object> parameters = null)
        {
            // only log info messages if the log level specified allows this
            if ((int)LogMessageType.Info <= logLevel)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                parameters.Add(Logger.LOGLOCATION, LogLocationEnum.OnEnter);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, INFORMATION, new List<TimeUnit>() { TimeUnit.Seconds });
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Info, applicationName, String.Empty, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogOnMethodSuccess(string methodName, Dictionary<string, object> parameters = null)
        {
            // only log info messages if the log level specified allows this
            if ((int)LogMessageType.Info <= logLevel)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                parameters.Add(Logger.LOGLOCATION, LogLocationEnum.OnComplete);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Info, applicationName, String.Empty, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogOnExitMethod(string methodName, Dictionary<string, object> parameters = null)
        {
            // only log info messages if the log level specified allows this
            if ((int)LogMessageType.Info <= logLevel)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                parameters.Add(Logger.LOGLOCATION, LogLocationEnum.OnExit);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Info, applicationName, String.Empty, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogOnMethodException(string methodName, Exception exception, Dictionary<string, object> parameters = null)
        {
            this.LogErrorMessageWithException(methodName, "", exception, parameters);
        }

        public void LogInfoMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            // only log info messages if the log level specified allows this
            if ((int)LogMessageType.Info <= logLevel)
            {
                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Info, applicationName, message, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogWarningMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            // only log warning messages if the log level specified allows this
            if ((int)LogMessageType.Warning <= logLevel)
            {
                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Warning, applicationName, message, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogDebugMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            // only log debug messages if the log level specified allows this
            if ((int)LogMessageType.Debug <= logLevel)
            {
                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Debug, applicationName, message, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogErrorMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            // only log error messages if the log level specified allows this
            if ((int)LogMessageType.Error <= logLevel)
            {
                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Error, applicationName, message, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, params object[] args)
        {
            this.LogFormattedError(methodName, formattedErrorMessage, null, args);
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            // only log error messages if the log level specified allows this
            if ((int)LogMessageType.Error <= logLevel)
            {
                string errorMessage = string.Empty;
                try
                {
                    errorMessage = string.Format(formattedErrorMessage, args);
                }
                catch
                {
                    errorMessage = "FAILURE: Failed to format error message string, logging has not completed correctly.";
                }
                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Error, applicationName, errorMessage, methodName);
                messageBus.Publish(logMessage);
            }
        }

        public void LogErrorMessageWithException(string methodName, string message, Exception exception, Dictionary<string, object> parameters = null)
        {
            // only log error messages if the log level specified allows this
            if ((int)LogMessageType.Error <= logLevel)
            {
                if (null == parameters)
                {
                    parameters = new Dictionary<string, object>();
                }

                var serializedException = Newtonsoft.Json.JsonConvert.SerializeObject(exception, Newtonsoft.Json.Formatting.Indented);

                parameters.Add(Logger.EXCEPTION, serializedException);
                this.ProcessThreadLocalParameters(parameters);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Error, applicationName, message, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, params object[] args)
        {
            this.LogFormattedErrorWithException(methodName, formattedErrorMessage, exception, null, args);
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, Dictionary<string, object> parameters, params object[] args)
        {
            // only log error messages if the log level specified allows this
            if ((int)LogMessageType.Error <= logLevel)
            {
                string errorMessage = string.Empty;
                try
                {
                    errorMessage = string.Format(formattedErrorMessage, args);
                }
                catch
                {
                    errorMessage = "FAILURE: Failed to format error message string, logging has not completed correctly.";
                }

                if (null == parameters)
                {
                    parameters = new Dictionary<string, object>();
                }

                var serializedException = Newtonsoft.Json.JsonConvert.SerializeObject(exception, Newtonsoft.Json.Formatting.Indented);

                parameters.Add(Logger.EXCEPTION, serializedException);
                this.ProcessThreadLocalParameters(parameters);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Error, applicationName, errorMessage, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, params object[] args)
        {
            this.LogFormattedWarning(methodName, formattedErrorMessage, null, args);
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            // only log warning messages if the log level specified allows this
            if ((int)LogMessageType.Warning <= logLevel)
            {
                string warningMessage = string.Empty;
                try
                {
                    warningMessage = string.Format(formattedErrorMessage, args);
                }
                catch
                {
                    warningMessage = "FAILURE: Failed to format warning message string, logging has not completed correctly.";
                }

                if (null == parameters)
                {
                    parameters = new Dictionary<string, object>();
                }
                this.ProcessThreadLocalParameters(parameters);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Warning, applicationName, warningMessage, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, params object[] args)
        {
            this.LogFormattedInfo(methodName, formattedErrorMessage, null, args);
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            // only log info messages if the log level specified allows this
            if ((int)LogMessageType.Info <= logLevel)
            {
                string infoMessage = string.Empty;
                try
                {
                    infoMessage = string.Format(formattedErrorMessage, args);
                }
                catch
                {
                    infoMessage = "FAILURE: Failed to format info message string, logging has not completed correctly.";
                }

                if (null == parameters)
                {
                    parameters = new Dictionary<string, object>();
                }
                this.ProcessThreadLocalParameters(parameters);

                //ThroughputMetricMessage metricMessage = new ThroughputMetricMessage(this.applicationName, methodName, new List<TimeUnit>() { TimeUnit.Seconds }, "", parameters);
                //messageBus.Publish(metricMessage);

                LogMessage logMessage = new LogMessage(LogMessageType.Info, applicationName, infoMessage, methodName, "", parameters);
                messageBus.Publish(logMessage);
            }
        }

        private void ProcessThreadLocalParameters(Dictionary<string, object> parameters)
        {
            Dictionary<string, object> threadParameters = Logger.ThreadContext;
            if (threadParameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in threadParameters)
                {
                    string parameterKey = kvp.Key;
                    if (parameters.ContainsKey(parameterKey))
                    {
                        parameterKey = string.Format("{0}_{1}", parameterKey, Guid.NewGuid());
                    }

                    parameters.Add(parameterKey, kvp.Value);
                }
            }
        }
    }
}