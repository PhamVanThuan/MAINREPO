using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.Maps.Config
{
    public interface IDomainProcess
    {
        T Get<T>() where T : IWorkflowService;
    }
}
