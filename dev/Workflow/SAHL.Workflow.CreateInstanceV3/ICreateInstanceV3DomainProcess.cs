using SAHL.Core.SystemMessages;
using SAHL.Workflow.Maps.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.CreateInstanceV3
{
    public interface ICreateInstanceV3DomainProcess : IWorkflowService
    {
        bool CreateCase(ISystemMessageCollection messages);
    }
}
