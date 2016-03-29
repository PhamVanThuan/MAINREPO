using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class RefreshCacheCommandHandler : IServiceCommandHandler<RefreshCacheCommand>
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IX2NodeConfigurationProvider x2NodeConfigurationProvider;
        private IProcessInstantiator processInstantiator;
        private IMessageCollectionFactory messageCollectionFactory;

        public RefreshCacheCommandHandler(IWorkflowDataProvider workflowDataProvider, IX2NodeConfigurationProvider x2NodeConfigurationProvider, IProcessInstantiator processInstantiator, IMessageCollectionFactory messageCollectionFactory)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.x2NodeConfigurationProvider = x2NodeConfigurationProvider;
            this.processInstantiator = processInstantiator;
            this.messageCollectionFactory = messageCollectionFactory;
        }

        public ISystemMessageCollection HandleCommand(RefreshCacheCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = messageCollectionFactory.CreateEmptyCollection();

            IEnumerable<X2Process> configuredProcesses = x2NodeConfigurationProvider.GetAvailableProcesses();
            IEnumerable<string> configuredProcessNames = configuredProcesses.Select(x => x.ProcessName);
            IEnumerable<ProcessViewModel> processModels = workflowDataProvider.GetConfiguredProcesses(configuredProcessNames);
            foreach (var processModel in processModels)
            {
#if DEBUG
                Console.WriteLine("Refreshing cache for process: {0}", processModel.ProcessName);
#endif
                var process = processInstantiator.GetProcess(processModel.ProcessID);
                var legacyProcess = process as IX2TLSLegacyProcess;
                if (legacyProcess != null)
                {
                    legacyProcess.ClearCache(messages,command.Data);
                }
            }

            return messages;
        }

    }
}
