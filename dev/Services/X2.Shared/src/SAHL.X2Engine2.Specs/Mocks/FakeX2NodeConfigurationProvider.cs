using SAHL.Core.Configuration;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Node.Providers;
using System;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class FakeX2NodeConfigurationProvider : ConfigurationProvider, IX2NodeConfigurationProvider
    {
        private string nodeExchangeName;
        private IX2RouteEndpoint engineRoute;
        private List<X2Process> processes;

        public FakeX2NodeConfigurationProvider(string nodeExchangeName, IX2RouteEndpoint routeEndpoint, string engineExchangeName, string engineQueueName, List<X2Process> processes)
        {
            this.nodeExchangeName = nodeExchangeName;
            this.engineRoute = new X2RouteEndpoint(engineExchangeName, engineQueueName);
            this.processes = processes;
        }

        public IX2RouteEndpoint GetEngineRoute()
        {
            return this.engineRoute;
        }

        public string GetExchangeName()
        {
            return this.engineRoute.ExchangeName;
        }

        public int IntervalToSendHealthMessagesInMilliseconds()
        {
            return 1;
        }

        public IEnumerable<X2Process> GetAvailableProcesses()
        {
            return processes;
        }

        public System.Configuration.Configuration Config
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<X2WorkflowConfiguration> GetWorkflowConfigurations()
        {
            throw new NotImplementedException();
        }
    }
}