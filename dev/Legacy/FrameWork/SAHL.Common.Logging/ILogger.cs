using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.Logging
{
    public interface ILogger
    {
        void LogOnEnterMethod(string methodName, Dictionary<string, object> parameters=null);

        void LogOnMethodSuccess(string methodName, Dictionary<string, object> parameters = null);

        void LogOnExitMethod(string methodName, Dictionary<string, object> parameters = null);

        void LogOnMethodException(string methodName, Exception exception, Dictionary<string, object> parameters = null);

        void LogInfoMessage(string methodName, string message, Dictionary<string, object> parameters = null);

        void LogFormattedInfo(string methodName, string formattedErrorMessage, params object[] args);
        void LogFormattedInfo(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args);

        void LogWarningMessage(string methodName, string message, Dictionary<string, object> parameters = null);

        void LogFormattedWarning(string methodName, string formattedErrorMessage, params object[] args);
        void LogFormattedWarning(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args);

        void LogDebugMessage(string methodName, string message, Dictionary<string, object> parameters = null);

        void LogErrorMessage(string methodName, string message, Dictionary<string, object> parameters = null);

        void LogFormattedError(string methodName, string formattedErrorMessage, params object[] args);
        void LogFormattedError(string methodName, string formattedErrorMessage, Dictionary<string, object> parameters, params object[] args);

        void LogErrorMessageWithException(string methodName, string message, Exception exception, Dictionary<string, object> parameters = null);

        void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, params object[] args);

        void LogFormattedErrorWithException(string methodName, string formattedErrorMessage, Exception exception, Dictionary<string, object> parameters, params object[] args);
    }
}