using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Factories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.EasyNetQ
{
    public class X2ConsumerMonitor : IX2ConsumerMonitor
    {
        private ConcurrentDictionary<X2Workflow, List<IX2RouteEndpoint>> workflowRoutes;
        private IX2QueueManager x2QueueManager;
        private IMessageBusManagementClient managementClient;
        private ITimer queueConsumerCheckTimer;
        private ITimerFactory timerFactory;

        public X2ConsumerMonitor(IX2QueueManager x2QueueManager, IMessageBusManagementClient managementClient, ITimerFactory timerFactory)
        {
            this.x2QueueManager = x2QueueManager;
            this.managementClient = managementClient;
            this.timerFactory = timerFactory;
        }

        public bool IsExchangeActive(string exchange, X2Workflow workflow)
        {
            var snapshot = Snapshot(workflowRoutes);
            List<IX2RouteEndpoint> x2RouteEndpoints = snapshot.FirstOrDefault(x => x.Key.WorkflowName == workflow.WorkflowName
                && x.Key.ProcessName == workflow.ProcessName).Value;

            if (x2RouteEndpoints == null ||
                !x2RouteEndpoints.Any())
                return false;
            else
                return x2RouteEndpoints.Any(x => x.ExchangeName == exchange);
        }

        private void UpdateWorkflowRoutes(object state)
        {
#if DEBUG
            Debug.WriteLine("updating active consumers per workflow");
#endif
            var activeQueues = this.managementClient.GetQueuesWithConsumers();
            foreach (var workflow in this.x2QueueManager.DeclaredWorkflowRoutes)
            {
                var routesWithNoActiveConsumers = workflow.Value.Select(x => x.QueueName).Except(activeQueues);
                var activeRoutes = workflow.Value.Where(x => !routesWithNoActiveConsumers.Contains(x.QueueName)).ToList();

                var snapshot = Snapshot(workflowRoutes);
                var keyToRemoveFrom = snapshot.FirstOrDefault(x => x.Key.WorkflowName == workflow.Key.WorkflowName && x.Key.ProcessName == workflow.Key.ProcessName).Key;
                var routesRemoved = new List<IX2RouteEndpoint>();
                workflowRoutes.TryRemove(keyToRemoveFrom, out routesRemoved);

                workflowRoutes.TryAdd(workflow.Key, activeRoutes);
#if DEBUG
                Debug.WriteLine("Updating workflow =  {0}.{1} -> active routes = {2}", workflow.Key.ProcessName, workflow.Key.WorkflowName, activeRoutes.Count.ToString());
#endif
            }
        }

        public void Initialise()
        {
            workflowRoutes = new ConcurrentDictionary<X2Workflow, List<IX2RouteEndpoint>>(this.x2QueueManager.DeclaredWorkflowRoutes.Select(
                x => new System.Collections.Generic.KeyValuePair<X2Workflow, List<IX2RouteEndpoint>>(x.Key, new List<IX2RouteEndpoint>())));

            UpdateWorkflowRoutes(null);

            queueConsumerCheckTimer = this.timerFactory.Get(UpdateWorkflowRoutes);
        }

        public void Dispose()
        {
            if (this.queueConsumerCheckTimer != null)
            {
                this.queueConsumerCheckTimer.Stop();
            }

            if (this.managementClient != null)
            {
                this.managementClient.Dispose();
            }
        }

        public Dictionary<X2Workflow, List<IX2RouteEndpoint>> Snapshot(ConcurrentDictionary<X2Workflow, List<IX2RouteEndpoint>> dictionary)
        {
            return dictionary.ToDictionary(x => x.Key,
                                                x => x.Value.Select(route => Cloner.Clone(route)).ToList<IX2RouteEndpoint>());
        }

        public static class Cloner
        {
            public static IX2RouteEndpoint Clone(IX2RouteEndpoint routeToClone)
            {
                return new X2RouteEndpoint(routeToClone.ExchangeName, routeToClone.QueueName);
            }
        }
    }
}

