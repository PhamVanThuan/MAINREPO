using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Communication
{
    public class X2QueueManager : IX2QueueManager
    {
        private IMessageBusAdvanced messageBus;
        private IX2QueueNameBuilder x2QueueNameBuilder;
        private IX2EngineConfigurationProvider engineConfigurationProvider;
        public const string X2EngineErrorExchange = "x2.error";
        public const string X2EngineErrorQueue = "x2.error";
        public const string X2EngineResponseExchange = "x2.response";
        public const string X2EngineResponseQueue = "x2.response";
        public const string X2EngineTimersRefreshExchange = "x2.timersrefresh";
        public const string X2EngineTimersRefreshQueue = "x2.timersrefresh";
        private Dictionary<X2Workflow, List<IX2RouteEndpoint>> workflowRoutes;

        public X2QueueManager(IMessageBusAdvanced messageBus, IX2QueueNameBuilder x2QueueNameBuilder, IX2EngineConfigurationProvider engineConfigurationProvider)
        {
            this.workflowRoutes = new Dictionary<X2Workflow, List<IX2RouteEndpoint>>();
            this.messageBus = messageBus;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
            this.engineConfigurationProvider = engineConfigurationProvider;
        }

        public Dictionary<X2Workflow, List<IX2RouteEndpoint>> DeclaredWorkflowRoutes
        {
            get { return workflowRoutes; }
        }

        private void DeclareTimersRefreshExchange()
        {
            this.messageBus.DeclareExchange(X2QueueManager.X2EngineTimersRefreshExchange);
        }

        private void DeclareTimersRefreshQueue()
        {
            this.messageBus.DeclareQueue(X2QueueManager.X2EngineTimersRefreshExchange, X2QueueManager.X2EngineTimersRefreshQueue, true);
        }

        private void DeclareResponseExchange()
        {
            this.messageBus.DeclareExchange(X2QueueManager.X2EngineResponseExchange);
        }

        private void DeclareResponseQueue()
        {
            this.messageBus.DeclareQueue(X2QueueManager.X2EngineResponseExchange, X2QueueManager.X2EngineResponseQueue, true);
        }

        private void DeclareErrorExchange()
        {
            this.messageBus.DeclareExchange(X2QueueManager.X2EngineErrorExchange);
        }

        private void DeclareErrorQueue()
        {
            this.messageBus.DeclareQueue(X2QueueManager.X2EngineErrorExchange, X2QueueManager.X2EngineErrorQueue, true);
        }

        private void DeclareWorkflowQueues()
        {
            foreach (var x2Workflow in engineConfigurationProvider.GetSupportedWorkflows())
            {
                var queues = x2QueueNameBuilder.GetQueues(x2Workflow);
                workflowRoutes.Add(x2Workflow, queues);
                foreach (var routeEndpoint in queues)
                {
                    this.messageBus.DeclareQueue(routeEndpoint.ExchangeName, routeEndpoint.QueueName, true);
                }
            }
        }

        private void DeclareWorkflowExchanges()
        {
            foreach (var x2Workflow in engineConfigurationProvider.GetSupportedWorkflows())
            {
                foreach (var exchange in x2QueueNameBuilder.GetExchanges(x2Workflow))
                {
                    this.messageBus.DeclareExchange(exchange);
                }
            }
        }

        public void Initialise()
        {
            DeclareErrorExchange();
            DeclareErrorQueue();
            DeclareResponseExchange();
            DeclareResponseQueue();
            DeclareWorkflowExchanges();
            DeclareWorkflowQueues();
            DeclareTimersRefreshExchange();
            DeclareTimersRefreshQueue();
        }
    }
}