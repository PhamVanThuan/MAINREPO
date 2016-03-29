using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SAHL.X2Engine2.Communication
{
    public class X2NodeRequestMessageProcessor : IMessageProcessor<IX2Request>
    {
        private IX2WorkflowRequestHandler workflowRequestHandler;
        private IX2ResponsePublisher responsePublisher;
        private IX2RequestInterrogator interrogator;
        private IX2QueueNameBuilder x2QueueNameBuilder;
        private IX2RequestPublisher requestPublisher;
        private IX2RequestStore x2RequestStore;
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        public X2NodeRequestMessageProcessor(IX2WorkflowRequestHandler workflowRequestHandler, IX2ResponsePublisher responsePublisher, IX2RequestInterrogator interrogator, IX2QueueNameBuilder x2QueueNameBuilder, IX2RequestPublisher requestPublisher, IX2RequestStore x2RequestStore, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.workflowRequestHandler = workflowRequestHandler;
            this.responsePublisher = responsePublisher;
            this.interrogator = interrogator;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
            this.requestPublisher = requestPublisher;
            this.x2RequestStore = x2RequestStore;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public Core.SystemMessages.ISystemMessageCollection ProcessMessage(IX2Request request)
        {
            X2Response response = null;
            X2Workflow workflow = null;
            long instanceId = request.InstanceId;
            var monitoredRequest = this.interrogator.IsRequestMonitored(request);
            List<IX2Request> completedRequests = new List<IX2Request>();

            switch (request.RequestType)
            {
                case X2RequestType.BundledRequest:
                    {
                        var bundledRequest = (X2BundledRequest)request;
                        var firstRequest = bundledRequest.Requests.First();
                        workflow = interrogator.GetRequestWorkflow(firstRequest);
                        instanceId = firstRequest.InstanceId;
                        foreach (var x2Request in bundledRequest.Requests)
                        {
                            response = InnerHandler(x2Request);
                            if (response.IsErrorResponse)
                            {
                                break;
                            }
                            completedRequests.Add(x2Request);
                        }
                        break;
                    }
                default:
                    {
                        workflow = interrogator.GetRequestWorkflow(request);
                        response = InnerHandler(request);
                        break;
                    }
            }

            if (!monitoredRequest && response.IsErrorResponse)
            {
                IX2Request errorQueueRequest = null;
                if (request.RequestType == X2RequestType.BundledRequest)
                {
                    var bundledRequest = (X2BundledRequest)request;
                    var requestsNotCompleted = bundledRequest.Requests.Where(x => !completedRequests.Contains(x)).ToList();
                    errorQueueRequest = new X2BundledRequest(requestsNotCompleted);
                }
                else
                {
                    errorQueueRequest = request;
                }

                if (errorQueueRequest != null)
                {
                    PublishErrorRequest(workflow, errorQueueRequest);
                    StoreAndLogErrorRequest(instanceId, errorQueueRequest.CorrelationId, response, errorQueueRequest);
                }
            }

            if (monitoredRequest)
            {
                IX2RouteEndpoint engineRoute = new X2RouteEndpoint(X2QueueManager.X2EngineResponseExchange, X2QueueManager.X2EngineResponseQueue);
                responsePublisher.Publish(engineRoute, response);
                if (response.IsErrorResponse)
                {
                    LogError(instanceId, response.RequestID, response);
                }
            }

            return SystemMessageCollection.Empty();
        }

        private void PublishErrorRequest(X2Workflow workflow, IX2Request errorQueueRequest)
        {
            var errorQueue = this.x2QueueNameBuilder.GetErrorQueue(workflow);
            requestPublisher.Publish<IX2Request>(errorQueue, errorQueueRequest);
        }


        private void StoreAndLogErrorRequest(long instanceId, Guid correlationId, X2Response response, IX2Request errorQueueRequest)
        {
            try
            {
                StoreErrorRequest(errorQueueRequest);
                LogError(instanceId, errorQueueRequest.CorrelationId, response);

            }
            catch (Exception)
            {
                IX2RouteEndpoint engineRoute = new X2RouteEndpoint(X2QueueManager.X2EngineErrorExchange, X2QueueManager.X2EngineErrorQueue);
                responsePublisher.Publish(engineRoute, response);
            }
        }

        private void StoreErrorRequest(IX2Request errorQueueRequest)
        {
            this.x2RequestStore.StoreReceivedRequest(new X2WrappedRequest(errorQueueRequest));
        }

        private void LogError(long instanceId, Guid correlationId, X2Response response)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("CorrelationId", correlationId);
            parameters.Add("InstanceId", instanceId);
            logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", "X2NodeRequestMessageProcessor", "Error while handling request", new Exception(response.Message), parameters);
        }

        private X2Response InnerHandler(IX2Request request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            X2Response response = null;
            response = this.workflowRequestHandler.Handle(request);
            stopWatch.Stop();

#if DEBUG
            Console.WriteLine(string.Format("Handle - Thread {0}, {1} - {2}", Thread.CurrentThread.ManagedThreadId, request.RequestType.ToString(), stopWatch.ElapsedMilliseconds));
#endif
            return response;
        }
    }
}