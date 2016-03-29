using System;
using SAHL.Core.Data;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.DomainProcessManagerProxy.Commands
{
    public class StartDomainProcessProxyCommand : ServiceCommand, IDomainProxyCommand
    {
        public string DataModel { get; set; }
        public string StartEventToWaitFor { get; set; }

        public StartDomainProcessProxyCommand(string dataModel, string startEventToWaitFor)
        {
            DataModel = dataModel;
            StartEventToWaitFor = startEventToWaitFor;
        }
    }
}
