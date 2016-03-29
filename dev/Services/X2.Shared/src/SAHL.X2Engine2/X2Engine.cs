using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.Providers;
using StructureMap;
using System;
using System.Reflection;

namespace SAHL.X2Engine2
{
    public class X2Engine : IX2Engine, IX2RequestMonitorCallback, IRouteRequest
    {
        private IX2RequestRouter requestRouter;
        private IX2RequestMonitorFactory requestMonitorFactory;
        private IResponseThreadWaiterManager threadWaiterManager;
        private IX2ResponseFactory responseFactory;
        private IX2EngineConfigurationProvider engineConfigurationProvider;
        private IRouteRequest storeAndRouteRequest;
        private IX2ActivityScheduler activityScheduler;
        private ISerializationProvider serializationProvider;
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;
        private IX2ConsumerManager x2ConsumerManager;
        private IX2NodeManagementPublisher x2NodeManagementPublisher;

        [DefaultConstructor]
        public X2Engine(IX2RequestRouter requestRouter, IX2RequestMonitorFactory requestMonitorFactory, IResponseThreadWaiterManager threadWaitManager, IX2ResponseFactory responseFactory, IX2EngineConfigurationProvider engineConfigurationProvider, IX2ActivityScheduler activityScheduler, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource, ISerializationProvider serializationProvider, IX2ConsumerManager x2ConsumerManager, IX2NodeManagementPublisher x2NodeManagementPublisher)
            : this(requestRouter, requestMonitorFactory, threadWaitManager, responseFactory, engineConfigurationProvider, null, activityScheduler, logger, loggerSource, loggerAppSource, serializationProvider, x2ConsumerManager, x2NodeManagementPublisher)
        {
        }

        public X2Engine(IX2RequestRouter requestRouter, IX2RequestMonitorFactory requestMonitorFactory, IResponseThreadWaiterManager threadWaitManager, IX2ResponseFactory responseFactory, IX2EngineConfigurationProvider engineConfigurationProvider, IRouteRequest storeAndRouteRequest, IX2ActivityScheduler activityScheduler, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource, ISerializationProvider serializationProvider, IX2ConsumerManager x2ConsumerManager, IX2NodeManagementPublisher x2NodeManagementPublisher)
        {
            this.requestRouter = requestRouter;
            this.requestMonitorFactory = requestMonitorFactory;
            this.threadWaiterManager = threadWaitManager;
            this.responseFactory = responseFactory;
            this.engineConfigurationProvider = engineConfigurationProvider;
            this.storeAndRouteRequest = storeAndRouteRequest ?? this;
            this.activityScheduler = activityScheduler;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
            this.serializationProvider = serializationProvider;
            this.x2ConsumerManager = x2ConsumerManager;
            this.x2NodeManagementPublisher = x2NodeManagementPublisher;
        }

        public void Initialise()
        {
            requestRouter.Initialise();
            requestMonitorFactory.Initialise();
            activityScheduler.Initialise(this);
            x2ConsumerManager.Initialise();
        }

        public IResponseThreadWaiter RouteRequest<T>(T request) where T : class, IX2Request
        {
            IResponseThreadWaiter threadWaiter = threadWaiterManager.Create(request);
            IX2RequestMonitor requestMonitor = requestMonitorFactory.GetOrCreateRequestMonitor(threadWaiter, this);
            requestMonitor.Initialise(this);
            requestRouter.RouteRequest<T>(request, requestMonitor);
            return threadWaiter;
        }

        public X2Response ReceiveRequest<T>(T request) where T : class, IX2Request
        {
            try
            {
                IResponseThreadWaiter threadWaiter = RouteRequest<T>(request);
                threadWaiter.Wait();
                return threadWaiter.Response;
            }
            catch (NoRoutesAvailableException nre)
            {
                string exceptionMessage = nre.Message;
                logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, nre);
                var messages = new SystemMessageCollection();
                messages.AddMessage(new SystemMessage("There are no routes available to process this request", SystemMessageSeverityEnum.Error));
                return this.responseFactory.CreateErrorResponse(request, nre.Message, request.InstanceId, new SystemMessageCollection());
            }
        }

        public X2Response ReceiveSystemRequest<T>(T request) where T : class, IX2SystemRequest
        {
            try
            {
                requestRouter.RouteRequest<T>(request, null);
                var response = this.responseFactory.CreateSuccessResponse(request, request.InstanceId, new SystemMessageCollection());
                return response;
            }
            catch (NoRoutesAvailableException nre)
            {
                string exceptionMessage = nre.Message;
                logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, nre);
                return this.responseFactory.CreateErrorResponse(request, "No routes available to process this request", 0, new SystemMessageCollection());
            }
        }

        public X2Response ReceiveManagementMessage<T>(T message) where T : class, IX2NodeManagementMessage
        {
            try
            {
                this.x2NodeManagementPublisher.Publish(message);
                var response = new X2Response(Guid.Empty,string.Empty,0);
                return response;
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, ex);
                return new X2ErrorResponse(Guid.Empty,ex.Message,null, new SystemMessageCollection());
            }
        }

        public void RequestCompleted(X2Response response)
        {
            var threadWaiterToRelease = threadWaiterManager.GetThreadWaiter(response.RequestID);
            if (threadWaiterToRelease != null)
            {
                threadWaiterToRelease.Continue(response);
                var requestMonitor = requestMonitorFactory.GetOrCreateRequestMonitor(threadWaiterToRelease, this);
                requestMonitor.Stop();
                requestMonitorFactory.RemoveRequestMonitor(threadWaiterToRelease);
                threadWaiterManager.Release(response.RequestID);
            }
        }

        public void RequestTimedout(IX2Request request, IResponseThreadWaiter threadWaiter)
        {
            var requestMonitor = requestMonitorFactory.GetOrCreateRequestMonitor(threadWaiter, this);
            requestMonitor.Stop();
            var response = responseFactory.CreateErrorResponse(request, "Request timed out and not servicable", request.InstanceId, new SystemMessageCollection());
            threadWaiter.Continue(response);
            requestMonitorFactory.RemoveRequestMonitor(threadWaiter);
        }

        public X2Response ReceiveExternalActivityRequest<T>(T request) where T : class, IX2ExternalActivityRequest
        {
            return ReceiveSystemRequest(request);
        }
    }
}