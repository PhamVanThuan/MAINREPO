using System.Collections.Generic;

namespace SAHL.X2Engine2.Communication
{
    public class X2QueueNameBuilder : IX2QueueNameBuilder
    {
        public X2QueueNameBuilder()
        { }

        public IX2RouteEndpoint GetUserQueue(Core.X2.Messages.X2Workflow workflow)
        {
            string[] x2QueueTypes = { X2QueueType.User };
            return GenerateQueues(workflow, x2QueueTypes)[0];
        }

        public IX2RouteEndpoint GetSystemQueue(Core.X2.Messages.X2Workflow workflow)
        {
            string[] x2QueueTypes = { X2QueueType.System };
            return GenerateQueues(workflow, x2QueueTypes)[0];
        }

        public IX2RouteEndpoint GetErrorQueue(Core.X2.Messages.X2Workflow workflow)
        {
            string[] x2QueueTypes = { X2QueueType.Error };
            return GenerateQueues(workflow, x2QueueTypes)[0];
        }

        public List<IX2RouteEndpoint> GetQueues(Core.X2.Messages.X2Workflow workflow)
        {
            string[] x2QueueTypes = { X2QueueType.User, X2QueueType.System, X2QueueType.Error };
            return GenerateQueues(workflow, x2QueueTypes);
        }

        public List<string> GetExchanges(Core.X2.Messages.X2Workflow workflow)
        {
            string[] x2QueueTypes = { X2QueueType.User, X2QueueType.System, X2QueueType.Error };
            return GenerateExchanges(workflow, x2QueueTypes);
        }

        public string GetUserExchange(Core.X2.Messages.X2Workflow workflow)
        {
            var processName = this.Sanitise(workflow.ProcessName);
            var workflowName = this.Sanitise(workflow.WorkflowName);
            return string.Format("x2.{0}.{1}.{2}", processName, workflowName, X2QueueType.User);
        }

        public string GetSystemExchange(Core.X2.Messages.X2Workflow workflow)
        {
            var processName = this.Sanitise(workflow.ProcessName);
            var workflowName = this.Sanitise(workflow.WorkflowName);
            return string.Format("x2.{0}.{1}.{2}", processName, workflowName, X2QueueType.System);
        }

        private List<IX2RouteEndpoint> GenerateQueues(Core.X2.Messages.X2Workflow workflow, string[] x2QueueTypes)
        {
            List<IX2RouteEndpoint> queues = new List<IX2RouteEndpoint>();
            foreach (var x2QueueType in x2QueueTypes)
            {
                var processName = this.Sanitise(workflow.ProcessName);
                var workflowName = this.Sanitise(workflow.WorkflowName);
                queues.Add(new X2RouteEndpoint(string.Format("x2.{0}.{1}.{2}", processName, workflowName, x2QueueType)
                    , string.Format("x2.{0}.{1}.{2}", processName, workflowName, x2QueueType)));
            }
            return queues;
        }

        private List<string> GenerateExchanges(Core.X2.Messages.X2Workflow workflow, string[] x2QueueTypes)
        {
            List<string> exchanges = new List<string>();
            var processName = this.Sanitise(workflow.ProcessName);
            var workflowName = this.Sanitise(workflow.WorkflowName);
            foreach (var x2QueueType in x2QueueTypes)
            {
                exchanges.Add(string.Format("x2.{0}.{1}.{2}", processName, workflowName, x2QueueType));
            }
            return exchanges;
        }

        private string Sanitise(string nameToSanitise)
        {
            return nameToSanitise.Replace(" ", "_").ToLower();
        }
    }
}