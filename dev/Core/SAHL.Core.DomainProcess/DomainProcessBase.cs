using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAHL.Core.DomainProcess
{
    public abstract class DomainProcessBase<TModel> : IDomainProcess<TModel> where TModel : class, IDataModel
    {
        private readonly IRawLogger rawLogger;
        private readonly ILoggerSource loggerSource;
        private readonly ILoggerAppSource loggerAppSource;

        public event EventHandler PersistState;

        public event DomainProcessEventhandler Completed;

        public event DomainProcessEventhandler ErrorOccurred;

        public DomainProcessStatus Status { get; protected set; }

        public string StatusReason { get; protected set; }

        protected DomainProcessBase(IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            if (rawLogger == null) { throw new ArgumentNullException("rawLogger"); }
            if (loggerSource == null) { throw new ArgumentNullException("loggerSource"); }
            if (loggerAppSource == null) { throw new ArgumentNullException("loggerAppSource"); }

            this.rawLogger = rawLogger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;

            this.DomainProcessId = CombGuid.Instance.Generate();
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
            this.Status = DomainProcessStatus.Processing;
        }

        protected string EventToWaitFor { get; private set; }

        protected TaskCompletionSource<IDomainProcessStartResult> StartTask { get; private set; }

        public Guid DomainProcessId { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public TModel DataModel { get; set; }

        public IDataModel ProcessState { get; set; }

        public IDataModel StartResultData { get; set; }

        public Task<IDomainProcessStartResult> Start(IDataModel datamodel, string eventToWaitFor)
        {
            if (datamodel == null) { throw new ArgumentNullException("datamodel"); }
            if (eventToWaitFor == null) { throw new ArgumentNullException("eventToWaitFor"); }

            try
            {
                this.StartTask = new TaskCompletionSource<IDomainProcessStartResult>();
                this.DataModel = datamodel as TModel;
                this.EventToWaitFor = eventToWaitFor;

                this.Initialise(datamodel);
                if (this.PersistState != null) { this.PersistState(this, null); }

                this.OnInternalStart();
            }
            catch (Exception runtimeException)
            {
                this.StartTask.SetException(runtimeException);
                OnErrorOccurred(this.DomainProcessId, runtimeException.ToString());
            }

            return this.StartTask.Task;
        }

        public abstract void Initialise(IDataModel dataModel);

        public abstract void OnInternalStart();

        public abstract void RestoreState(IDataModel dataModel);

        public void HandleEvent(IEvent domainProcessEvent, IServiceRequestMetadata metadata)
        {
            if (domainProcessEvent == null) { throw new ArgumentNullException("domainProcessEvent"); }

            var handlerMethod = this.FindDomainProcessHandlerMethod(domainProcessEvent);
            try
            {
                handlerMethod.Invoke(this, new object[] { domainProcessEvent, metadata });

                if (this.IsStartEventWaitedFor(domainProcessEvent) || this.EventToWaitFor.Equals("FireAndForget", StringComparison.Ordinal))
                {
                    this.SetStartResult(true, this.StartResultData);
                    if (this.EventToWaitFor.Equals("FireAndForget", StringComparison.Ordinal))
                    {
                        this.EventToWaitFor = "FiredAndForgotten";
                    }
                }

                this.HandledEvent(metadata);
            }
            catch (Exception runtimeException)
            {
                this.HandleEventException(domainProcessEvent, metadata, runtimeException.InnerException ?? runtimeException);
            }
        }

        public IDataModel GetDataModel()
        {
            return this.DataModel;
        }

        public void SetDataModel(IDataModel dataModel)
        {
            this.DataModel = dataModel as TModel;
        }

        public abstract void HandledEvent(IServiceRequestMetadata metadata);

        protected void OnCompleted(Guid domainProcessId)
        {
            this.Status = DomainProcessStatus.Complete;
            if (this.Completed == null) { return; }
            this.Completed(domainProcessId);
        }

        protected void OnCompletedWithErrors(Guid domainProcessId, string errorMessage)
        {
            this.Status = DomainProcessStatus.Complete;
            this.StatusReason = errorMessage;
            if (this.Completed == null) { return; }
            this.Completed(domainProcessId);
        }

        protected void OnErrorOccurred(Guid domainProcessId, string errorMessage)
        {
            this.Status = DomainProcessStatus.Error;
            this.StatusReason = errorMessage;
            if (this.ErrorOccurred == null) { return; }
            this.ErrorOccurred(domainProcessId);
        }

        protected void LogErrorMessage(string errorMessage)
        {
            var message = String.Format("Domain Process {0} : {1}", this.DomainProcessId, errorMessage);
            rawLogger.LogError(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                     "System", string.Empty, message);
        }

        protected void LogInfoMessage(string infoMessage)
        {
            var message = String.Format("Domain Process {0} : {1}", this.DomainProcessId, infoMessage);
            rawLogger.LogInfo(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName,
                     "System", string.Empty, message);
        }

        private bool IsStartEventWaitedFor(IEvent domainProcessEvent)
        {
            return domainProcessEvent.GetType().Name == this.EventToWaitFor;
        }

        private void SetStartResult(bool startResult, IDataModel startData)
        {
            var domainProcessStartResult = new DomainProcessStartResult(startResult, startData);
            this.StartTask.SetResult(domainProcessStartResult);
        }

        private MethodInfo FindDomainProcessHandlerMethod(IEvent domainProcessEvent)
        {
            var allMethods = this.GetType().GetMethods();

            var handleMethod = (from currentMethod in allMethods
                                where currentMethod.Name == "Handle"
                                let allParameters = currentMethod.GetParameters()
                                where allParameters.Any()
                                where allParameters.Any(parameterInfo => parameterInfo.ParameterType.Name == domainProcessEvent.GetType().Name)
                                select currentMethod).FirstOrDefault();

            if (handleMethod == null)
            {
                throw new Exception(string.Format("Domain Process Handler event not found [{0}.{1}]", this.GetType().Name,
                                                                                                      domainProcessEvent.Name));
            }

            return handleMethod;
        }

        private void HandleEventException(IEvent domainProcessEvent, IServiceRequestMetadata metadata, Exception runtimeException)
        {
            var handleExceptionMethod = this.FindDomainProcessHandleExceptionMethod(domainProcessEvent);
            if (handleExceptionMethod == null)
            {
                if (this.StartTask != null && !this.StartTask.Task.IsCompleted)
                {
                    this.StartTask.SetException(runtimeException);
                }
                this.OnErrorOccurred(this.DomainProcessId, runtimeException.ToString());
                throw runtimeException;
            }
            else
            {
                handleExceptionMethod.Invoke(this, new object[] { domainProcessEvent, metadata, runtimeException });
            }
        }

        private MethodInfo FindDomainProcessHandleExceptionMethod(IEvent domainProcessEvent)
        {
            var allMethods = this.GetType().GetMethods();

            allMethods = (from currentMethod in allMethods
                          where currentMethod.Name == "HandleException"
                          let allParameters = currentMethod.GetParameters()
                          where allParameters.Any()
                          select currentMethod).ToArray();

            var handleMethod = (from currentMethod in allMethods
                                where currentMethod.GetParameters().Any(parameterInfo => parameterInfo.ParameterType.Name == domainProcessEvent.GetType().Name)
                                select currentMethod).FirstOrDefault();

            if (handleMethod == null)
            {
                handleMethod = (from currentMethod in allMethods
                                where currentMethod.GetParameters().Any(parameterInfo => parameterInfo.ParameterType.Name == "IEvent")
                                select currentMethod).FirstOrDefault();
            }


            return handleMethod;
        }
    }
}