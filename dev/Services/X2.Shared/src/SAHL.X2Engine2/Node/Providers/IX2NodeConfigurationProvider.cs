using SAHL.Core.Configuration;
using SAHL.Core.X2.Messages;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Node.Providers
{
    public interface IX2NodeConfigurationProvider : IConfigurationProvider
    {
        IEnumerable<X2Process> GetAvailableProcesses();

        IEnumerable<X2WorkflowConfiguration> GetWorkflowConfigurations();

        string GetExchangeName();
    }
}