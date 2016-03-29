using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloWizardWorkflowConfiguration : IHaloWizardTileConfiguration
    {
        string ProcessName { get; }
        string WorkflowName { get; }
        string ActivityName { get; }
    }
}
