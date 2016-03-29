using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Shared.Repository
{
    public interface ITileWizardConfigurationRepository
    {
        IHaloWizardTileConfiguration FindWizardConfiguration(string wizardName);
        IHaloWizardWorkflowConfiguration FindWizardWorkflowConfiguration(string processName, string workflowName, string activityName);
        IEnumerable<IHaloWizardTilePageConfiguration> FindWizardTilePageConfigurations<T>(T wizardTileConfiguration) where T : IHaloWizardTileConfiguration;
        IHaloTileModel FindWizardPageDataModel<T>(T pageConfiguration) where T : class, IHaloWizardTilePageConfiguration;
        IHaloTileState FindWizardPageState<T>(T tileConfiguration) where T : class, IHaloWizardTilePageConfiguration;
    }
}
