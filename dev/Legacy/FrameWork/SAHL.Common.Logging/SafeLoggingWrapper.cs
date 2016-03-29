using System;
using System.Collections.Generic;

namespace SAHL.Common.Logging
{
    public class SafeLoggingWrapper : ILogger
    {
        public SafeLoggingWrapper(ILogger innerLogger)
        {
            this.InnerLogger = innerLogger;
        }

        public ILogger InnerLogger { get; protected set; }

        public void LogOnEnterMethod(string methodName, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogOnEnterMethod(methodName, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogOnMethodSuccess(string methodName, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogOnMethodSuccess(methodName, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogOnExitMethod(string methodName, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogOnExitMethod(methodName, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogOnMethodException(string methodName, Exception exception, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogOnMethodException(methodName, exception, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogInfoMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogInfoMessage(methodName, message, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedInfo(methodName, formattedErrorMessage, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedInfo(methodName, formattedErrorMessage, parameters, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogWarningMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogWarningMessage(methodName, message, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedWarning(methodName, formattedErrorMessage, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedWarning(methodName, formattedErrorMessage, parameters, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogDebugMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogDebugMessage(methodName, message, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogErrorMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogErrorMessage(methodName, message, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedError(methodName, formattedErrorMessage, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedError(methodName, formattedErrorMessage, parameters, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogErrorMessageWithException(string methodName, string message, Exception exception, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogErrorMessageWithException(methodName, message, exception, parameters);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedErrorWithException(methodName, formattedErrorMessage, exception, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, Dictionary<string, object> parameters, params object[] args)
        {
            try
            {
                if (this.InnerLogger != null)
                {
                    this.InnerLogger.LogFormattedErrorWithException(methodName, formattedErrorMessage, exception, parameters, args);
                }
            }
            catch
            {
                // fail silently when logging
            }
        }
    }
}