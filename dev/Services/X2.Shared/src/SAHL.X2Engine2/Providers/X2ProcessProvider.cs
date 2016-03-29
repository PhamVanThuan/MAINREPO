using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Providers
{
    public class X2ProcessProvider : IX2ProcessProvider
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IX2NodeConfigurationProvider x2NodeConfigurationProvider;
        private IProcessInstantiator processInstantiator;

        public X2ProcessProvider(IWorkflowDataProvider workflowDataProvider, IX2NodeConfigurationProvider x2NodeConfigurationProvider, IProcessInstantiator processInstantiator)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.x2NodeConfigurationProvider = x2NodeConfigurationProvider;
            this.processInstantiator = processInstantiator;
        }

        public void Initialise()
        {
            IEnumerable<X2Process> configuredProcesses = x2NodeConfigurationProvider.GetAvailableProcesses();
            IEnumerable<string> configuredProcessNames = configuredProcesses.Select(x => x.ProcessName);
            IEnumerable<ProcessViewModel> processModels = workflowDataProvider.GetConfiguredProcesses(configuredProcessNames);
            foreach (var processModel in processModels)
            {
#if DEBUG
                Console.WriteLine("Starting Process: {0}", processModel.ProcessName);
#endif
                processInstantiator.GetProcess(processModel.ProcessID);
            }
        }

        public IX2Process GetProcessForInstance(long instanceID)
        {
            InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(instanceID);
            WorkFlowDataModel workflow = workflowDataProvider.GetWorkflow(instance);
            ProcessDataModel processDataModel = workflowDataProvider.GetProcessById(workflow.ProcessID);
            long tics = DateTime.Now.Ticks;
            IX2Process process = processInstantiator.GetProcess(processDataModel.ID);
            return process;
        }
    }
}