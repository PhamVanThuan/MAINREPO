using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database
{
    public class DatabaseLogger : IRawLogger
    {
        private readonly IDatabaseLoggerDataManager databaseLoggerDataManager;
        private readonly IDatabaseLoggerUtils databaseLoggerUtils;

        public DatabaseLogger(IDatabaseLoggerDataManager databaseLoggerDataManager, IDatabaseLoggerUtils databaseLoggerUtils)
        {
            this.databaseLoggerDataManager = databaseLoggerDataManager;
            this.databaseLoggerUtils = databaseLoggerUtils;
        }

        public void LogStartup(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);

            // only log startup messages if the log level specified allows this
            if (LogLevel.StartUp <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.StartUp, source, applicationName, user, methodName, message, parameters);
            }
        }

        // + Error Messages

        public void LogError(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);

            // only log error messages if the log level specified allows this
            if (LogLevel.Error <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.Error, source, applicationName, user, methodName, message, parameters);
            }
        }

        public void LogFormattedError(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, 
                                      IDictionary<string, object> parameters, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogError(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        public void LogFormattedError(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogError(logLevel, source, applicationName, user, methodName, message, null);
        }

        public void LogErrorWithException(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, Exception exception, 
                                          IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.EnsureParametersCreated(ref parameters);
            var exceptionData = this.databaseLoggerUtils.SerializeObject(exception);
            parameters.Add(Logger.Exception, exceptionData);
            this.LogError(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        public void LogFormattedErrorWithException(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, 
                                                   Exception exception, IDictionary<string, object> parameters, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.databaseLoggerUtils.EnsureParametersCreated(ref parameters);
            var exceptionData = this.databaseLoggerUtils.SerializeObject(exception);
            parameters.Add(Logger.Exception, exceptionData);
            this.LogError(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        public void LogFormattedErrorWithException(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, 
                                                   Exception exception, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            var exceptionData = this.databaseLoggerUtils.SerializeObject(exception);
            parameters.Add(Logger.Exception, exceptionData);
            this.LogError(logLevel, source, applicationName, methodName, message, user, parameters);
        }

        // - Error Messsages

        // + Info Messages

        public void LogInfo(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);

            // only log info messages if the log level specified allows this
            if (LogLevel.Info <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.Information, source, applicationName, user, methodName, message, parameters);
            }
        }

        public void LogFormattedInfo(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, 
                                     IDictionary<string, object> parameters, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogInfo(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        public void LogFormattedInfo(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogInfo(logLevel, source, applicationName, methodName, message, user, null);
        }

        // - Info Messages

        // + Warning Messages

        public void LogWarning(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);

            // only log warning messages if the log level specified allows this
            if (LogLevel.Warning <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.Warning, source, applicationName, user, methodName, message, parameters);
            }
        }

        public void LogFormattedWarning(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, 
                                        IDictionary<string, object> parameters, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogWarning(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        public void LogFormattedWarning(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedErrorMessage, params object[] args)
        {
            var message = string.Format(formattedErrorMessage, args);
            this.LogWarning(logLevel, source, applicationName, user, methodName, message, null);
        }

        // - Warning Messages

        // + Debug Messages

        public void LogDebug(LogLevel logLevel, string source, string applicationName, string user, string methodName, string message, IDictionary<string, object> parameters = null)
        {
            this.databaseLoggerUtils.MergeThreadLocalParameters(ref parameters);

            // only log debug messages if the log level specified allows this
            if (LogLevel.Debug <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.Debug, source, applicationName, user, methodName, message, parameters);
            }
        }

        public void LogFormattedDebug(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedDebugMessage, params object[] args)
        {
            var message = string.Format(formattedDebugMessage, args);
            this.LogDebug(logLevel, source, applicationName, user, methodName, message);
        }

        public void LogFormattedDebug(LogLevel logLevel, string source, string applicationName, string user, string methodName, string formattedDebugMessage, 
                                      IDictionary<string, object> parameters, params object[] args)
        {
            var message = string.Format(formattedDebugMessage, args);
            this.LogDebug(logLevel, source, applicationName, user, methodName, message, parameters);
        }

        // - Debug Messages

        public void LogOnEnterMethod(LogLevel logLevel, string source, string applicationName, string user, string methodName, IDictionary<string, object> parameters = null)
        {
            if (LogLevel.Debug <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.MethodEnter, source, applicationName, user, methodName, string.Empty, parameters);
            }
        }

        public void LogOnExitMethod(LogLevel logLevel, string source, string applicationName, string user, string methodName, IDictionary<string, object> parameters = null)
        {
            if (LogLevel.Debug <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.MethodExit, source, applicationName, user, methodName, string.Empty, parameters);
            }
        }

        public void LogOnMethodException(LogLevel logLevel, string source, string applicationName, string user, string methodName, Exception exception, 
                                         IDictionary<string, object> parameters = null)
        {
            var exceptionMessage = this.databaseLoggerUtils.SerializeObject(exception);

            if (LogLevel.Debug <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.MethodException, source, applicationName, user, methodName, exceptionMessage, parameters);
            }
        }

        public void LogOnMethodSuccess(LogLevel logLevel, string source, string applicationName, string user, string methodName, IDictionary<string, object> parameters = null)
        {
            if (LogLevel.Debug <= logLevel)
            {
                this.databaseLoggerDataManager.StoreLog(Logger.MethodSuccess, source, applicationName, user, methodName, string.Empty, parameters);
            }
        }

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return Logger.ThreadContext;
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            foreach (var kvp in threadContext)
            {
                if (!Logger.ThreadContext.ContainsKey(kvp.Key))
                {
                    Logger.ThreadContext.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    threadContext[kvp.Key] = kvp.Value;
                }
            }
        }

        private string FormatMessage(string message, params object[] args)
        {
            string formattedMessage = string.Empty;
            try
            {
                formattedMessage = string.Format(message, args);
            }
            catch
            {
                formattedMessage = "FAILURE: Failed to format info message string, logging has not completed correctly.";
            }

            return formattedMessage;
        }
    }
}