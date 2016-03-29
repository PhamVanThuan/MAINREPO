using SAHL.Core.Configuration;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System.Collections.Generic;
using System.Configuration;

namespace SAHL.X2Engine2.Node.Providers
{
    public class X2NodeConfigurationProvider : ConfigurationProvider, IX2NodeConfigurationProvider
    {
        private ISerializationProvider serializationProvider;

        public X2NodeConfigurationProvider(ISerializationProvider serializationProvider)
            : base()
        {
            this.serializationProvider = serializationProvider;
        }

        public IEnumerable<X2WorkflowConfiguration> GetWorkflowConfigurations()
        {
            string jSon = ConfigurationManager.AppSettings["supportedProcesses"];
            List<X2ProcessConfiguration> supportedProcesses = serializationProvider.Deserialize<List<X2ProcessConfiguration>>(jSon);

            List<X2WorkflowConfiguration> workflowConfigurations = new List<X2WorkflowConfiguration>();
            foreach (X2ProcessConfiguration x2Process in supportedProcesses)
            {
                workflowConfigurations.AddRange(x2Process.WorkflowConfigurations);
            }
            return workflowConfigurations;
        }

        public string GetExchangeName()
        {
            return base.Config.AppSettings.Settings["nodeExchangeName"].Value;
        }

        public IEnumerable<X2Process> GetAvailableProcesses()
        {
            string jSon = ConfigurationManager.AppSettings["supportedProcesses"];
            List<X2Process> supportedProcesses = serializationProvider.Deserialize<List<X2Process>>(jSon);
            return supportedProcesses;
        }
    }
}