using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloWorkflowAction : IHaloAction
    {
        string ProcessName { get; }
        string WorkflowName { get; }
        long InstanceId { get; }
    }
}
