using System;
using System.Collections.Generic;

namespace SAHL.Common.Logging
{
    public class DefaultLogger : ILogger
    {
        public void LogOnEnterMethod(string methodName, Dictionary<string, object> parameters = null)
        {
        }

        public void LogOnMethodSuccess(string methodName, Dictionary<string, object> parameters = null)
        {
        }

        public void LogOnExitMethod(string methodName, Dictionary<string, object> parameters = null)
        {
        }

        public void LogOnMethodException(string methodName, Exception exception, Dictionary<string, object> parameters = null)
        {
        }

        public void LogInfoMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, params object[] args)
        {
        }

        public void LogFormattedInfo(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
        }

        public void LogWarningMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, params object[] args)
        {
        }

        public void LogFormattedWarning(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
        }

        public void LogDebugMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
        }

        public void LogErrorMessage(string methodName, string message, Dictionary<string, object> parameters = null)
        {
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, params object[] args)
        {
        }

        public void LogFormattedError(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args)
        {
        }

        public void LogErrorMessageWithException(string methodName, string message, Exception exception, Dictionary<string, object> parameters = null)
        {
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, params object[] args)
        {
        }

        public void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, Dictionary<string, object> parameters, params object[] args)
        {
        }
    }
}