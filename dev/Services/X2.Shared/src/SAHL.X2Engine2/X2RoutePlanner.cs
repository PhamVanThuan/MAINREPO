using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAHL.X2Engine2
{
    public class X2RoutePlanner : IX2RoutePlanner
    {
        private IX2EngineConfigurationProvider engineConfigurationProvider;
        private IX2QueueManager x2QueueManager;
        private IX2QueueNameBuilder x2QueueNameBuilder;
        private IX2ConsumerMonitor consumerMonitor;
        
        public X2RoutePlanner(IX2EngineConfigurationProvider engineConfigurationProvider, IX2QueueManager x2QueueManager, IX2QueueNameBuilder x2QueueNameBuilder, IX2ConsumerMonitor consumerMonitor)
        {
            this.engineConfigurationProvider = engineConfigurationProvider;
            this.x2QueueManager = x2QueueManager;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
            this.consumerMonitor = consumerMonitor;
        }

        public void Initialise()
        {
            this.x2QueueManager.Initialise();
            this.consumerMonitor.Initialise();
        }

        public IX2RouteEndpoint PlanRoute(bool monitoredRequest, X2Workflow workflow)
        {
            var exchange = string.Empty;

            if (monitoredRequest)
            {
                exchange = this.x2QueueNameBuilder.GetUserExchange(workflow);
            }
            else
            {
                exchange = this.x2QueueNameBuilder.GetSystemExchange(workflow);
            }

            if (monitoredRequest && !this.consumerMonitor.IsExchangeActive(exchange, workflow))
            {
                return null;
            }
            var queue = exchange;
            return new X2RouteEndpoint(exchange, queue);
        }
    }
}