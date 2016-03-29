using SAHL.Core.Logging;
using SAHL.Core.X2.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowMaps.Specs.Common
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.For<IX2Logging>().Use<TestX2Logging>();
                    x.For<ILogger>().Use<TestLogger>();
                    x.For<SAHL.Core.Logging.ILoggerSource>().Use<TestLoggerSource>();
                });
        }
    }

    public class TestLoggerSource : SAHL.Core.Logging.ILoggerSource
    {
        public Guid Id
        {
            get { return Guid.NewGuid(); }
        }

        public LogLevel LogLevel
        {
            get { return LogLevel.Error; }
        }

        public bool LogMetrics
        {
            get { return false; }
        }

        public string Name
        {
            get { return "Name"; }
        }



        LogLevel ILoggerSource.LogLevel
        {
            get
            {
                return SAHL.Core.Logging.LogLevel.Info;
            }
            set
            {

            }
        }

        bool ILoggerSource.LogMetrics
        {
            get
            {
                return true;
            }
            set
            {

            }
        }
    }


    public class TestLogger : SAHL.Core.Logging.ILogger
    {

        public void LogDebug(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogError(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogErrorWithException(ILoggerSource loggerSource, string user, string method, string message, Exception exception, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method, string formattedDebugMessage, IDictionary<string, object> parameters, params object[] args)
        {
            
        }

        public void LogFormattedDebug(ILoggerSource loggerSource, string user, string method, string formattedDebugMessage, params object[] args)
        {
            
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            
        }

        public void LogFormattedError(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, Exception exception, IDictionary<string, object> parameters, params object[] args)
        {
            
        }

        public void LogFormattedErrorWithException(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, Exception exception, params object[] args)
        {
            
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            
        }

        public void LogFormattedInfo(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, IDictionary<string, object> parameters, params object[] args)
        {
            
        }

        public void LogFormattedWarning(ILoggerSource loggerSource, string user, string method, string formattedErrorMessage, params object[] args)
        {
            
        }

        public void LogInfo(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value, IDictionary<string, object> parameters)
        {
            
        }

        public void LogInstantaneousValueMetric(ILoggerSource loggerSource, string user, string metric, int value)
        {
            
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration, IDictionary<string, object> parameters)
        {
            
        }

        public void LogLatencyMetric(ILoggerSource loggerSource, string user, string metric, DateTime startTime, TimeSpan duration)
        {
            
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric, IDictionary<string, object> parameters)
        {
            return default(T);
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric, IDictionary<string, object> parameters)
        {
            
        }

        public T LogMethodMetric<T>(ILoggerSource loggerSource, string user, string metric, Func<T> actionToMetric)
        {
            return default(T);
        }

        public void LogMethodMetric(ILoggerSource loggerSource, string user, string metric, Action actionToMetric)
        {
            
        }

        public void LogOnEnterMethod(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogOnExitMethod(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogOnMethodException(ILoggerSource loggerSource, string user, string method, Exception exception, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogOnMethodSuccess(ILoggerSource loggerSource, string user, string method, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogStartup(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric, IDictionary<string, object> parameters)
        {
            
        }

        public void LogThroughputMetric(ILoggerSource loggerSource, string user, string metric)
        {
            
        }

        public void LogWarning(ILoggerSource loggerSource, string user, string method, string message, IDictionary<string, object> parameters = null)
        {
            
        }

        public IDictionary<string, object> GetThreadLocalStore()
        {
            return new Dictionary<string, object>();
        }

        public void SetThreadLocalStore(IDictionary<string, object> threadContext)
        {
            
        }
    }

    public class TestX2Logging : SAHL.Core.X2.Logging.IX2Logging
    {
        public void LogOnEnterWorkflow(string methodName, object workflowData, SAHL.Core.Data.Models.X2.InstanceDataModel instanceData, SAHL.Core.X2.IX2Params X2params, Dictionary<string, object> parameters)
        {
            
        }

        public void LogOnEnterWorkflow(string methodName, object workflowData, SAHL.Core.Data.Models.X2.InstanceDataModel instanceData, SAHL.Core.X2.IX2Params X2params)
        {
            
        }

        public void LogOnEnterX2Request(string methodName, long instanceId, SAHL.Core.X2.Messages.X2Request request, string ADUser)
        {
            
        }

        public void LogOnExitWorkflow(string methodName, long instanceId)
        {
            
        }

        public void LogOnExitX2Request(string methodName, long instanceId)
        {
            
        }

        public void LogOnWorkflowException(string methodName, long instanceId, Exception exception)
        {
            
        }

        public void LogOnWorkflowSuccess(string methodName, object workflowData, SAHL.Core.Data.Models.X2.InstanceDataModel instanceData, SAHL.Core.X2.IX2Params X2params)
        {
            
        }

        public void LogOnX2RequestException(string methodName, long instanceId, Exception exception, SAHL.Core.X2.Messages.X2ErrorResponse errorResponse)
        {
            
        }

        public void LogOnX2RequestSuccess(string methodName, long instanceId, SAHL.Core.X2.Messages.X2Response response)
        {
            
        }

        public void LogOnEnterWorkflow(string methodName, object workflowData, SAHL.Core.Data.Models.X2.InstanceDataModel instanceData, SAHL.Core.X2.IX2Params X2params, IDictionary<string, object> parameters)
        {
        }
    }
}
