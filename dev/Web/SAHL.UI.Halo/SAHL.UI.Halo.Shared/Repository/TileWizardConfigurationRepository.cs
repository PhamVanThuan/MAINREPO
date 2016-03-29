using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Shared.Repository
{
    public class TileWizardConfigurationRepository : ITileWizardConfigurationRepository
    {
        private readonly IIocContainer iocContainer;

        public TileWizardConfigurationRepository(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            this.iocContainer = iocContainer;
        }

        public IHaloWizardTileConfiguration FindWizardConfiguration(string wizardName)
        {
            if (string.IsNullOrWhiteSpace(wizardName)) {  throw new ArgumentNullException("wizardName"); }

            var wizardConfiguration = this.iocContainer.GetInstance<IHaloWizardTileConfiguration>(wizardName);
            return wizardConfiguration;
        }

        public IHaloWizardWorkflowConfiguration FindWizardWorkflowConfiguration(string processName, string workflowName, string activityName)
        {
            if (string.IsNullOrWhiteSpace(processName)) { throw new ArgumentNullException("processName"); }
            if (string.IsNullOrWhiteSpace(workflowName)) { throw new ArgumentNullException("workflowName"); }
            if (string.IsNullOrWhiteSpace(activityName)) { throw new ArgumentNullException("activityName"); }

            var wizardConfigurations = this.iocContainer.GetAllInstances<IHaloWizardWorkflowConfiguration>();
            if (!wizardConfigurations.Any()) { return null; }

            var workflowWizard = wizardConfigurations.FirstOrDefault(configuration => configuration.ProcessName == processName &&
                                                                                      configuration.WorkflowName == workflowName &&
                                                                                      configuration.ActivityName == activityName);
            return workflowWizard;
        }

        public IEnumerable<IHaloWizardTilePageConfiguration> FindWizardTilePageConfigurations<T>(T wizardTileConfiguration) where T : IHaloWizardTileConfiguration
        {
            var tileType        = wizardTileConfiguration.GetType();
            var contentProvider = typeof(IHaloWizardTilePageConfiguration<>);
            var genericType     = contentProvider.MakeGenericType(tileType);

            var wizardPages = this.iocContainer.GetAllInstances(genericType);
            if (!wizardPages.Any()) { return null; }

            IEnumerable<IHaloWizardTilePageConfiguration> orderedConfigurations = wizardPages.Cast<IHaloWizardTilePageConfiguration>()
                                                                                             .OrderBy(configuration => configuration.Sequence);
            return orderedConfigurations;
        }

        public IHaloTileModel FindWizardPageDataModel<T>(T pageConfiguration) where T : class, IHaloWizardTilePageConfiguration
        {
            var tileModelInterface = pageConfiguration.GetType().GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTileModel"));
            if (tileModelInterface == null) { return null; }

            var dataModelType = tileModelInterface.GenericTypeArguments[0];
            return Activator.CreateInstance(dataModelType) as IHaloTileModel;
        }

        public IHaloTileState FindWizardPageState<T>(T tileConfiguration) where T : class, IHaloWizardTilePageConfiguration
        {
            var tilePageState = tileConfiguration.GetType().GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloTilePageState"));
            if (tilePageState == null) { return null; }

            var tileStateType = tilePageState.GenericTypeArguments[0];
            return Activator.CreateInstance(tileStateType) as IHaloTileState;
        }

    }
}
